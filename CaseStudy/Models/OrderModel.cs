using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class TrayModel
    {
        private AppDbContext _db;
        public TrayModel(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Tray> GetAll(ApplicationUser user)
        {
            return _db.Trays.Where(tray => tray.UserId == user.Id).ToList<Tray>();
        }
        public List<TrayViewModel> GetTrayDetails(int tid, string uid)
        {
            List<TrayViewModel> allDetails = new List<TrayViewModel>();
            // LINQ way of doing INNER JOINS
            var results = from t in _db.Set<Tray>()
                          join ti in _db.Set<TrayItem>() on t.Id equals ti.OrderId
                          join mi in _db.Set<Product>() on ti.ProductId equals mi.Id
                          where (t.UserId == uid && t.Id == tid)
                          select new TrayViewModel
                          {
                              TrayId = mi.Id,
                              UserId = uid,
                              Description = mi.Description,
                              QtyO = ti.QtyOrdered,
                              Name = ti.Product.ProductName,
                              Price = mi.MSRP,                              
                              QtyS = ti.QtySold,
                              QtyB = ti.QtyBackOrdered,
                              Extended = mi.MSRP * ti.QtySold,
                              Sub = (double)t.OrderAmount,
                              Tax = (double)t.OrderAmount * 0.13,
                              Total = t.OrderAmount * (decimal)1.13,
                              DateCreated = t.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = results.ToList<TrayViewModel>();
            return allDetails;
        }
        public int AddTray(Dictionary<string, object> items, ApplicationUser user, bool itemsBackOrdered)
        {
            int trayId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {                    try
                    {
                        Tray tray = new Tray();
                        tray.Id = 0;
                        tray.UserId = user.Id;
                        tray.OrderDate = System.DateTime.Now;
                        tray.OrderAmount = 0;
                        // calculate the totals and then add the tray row to the table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item = JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                tray.OrderAmount += item.MSRP * item.Qty;
                            }
                        }
                        _db.Trays.Add(tray);
                        _db.SaveChanges();
                        // then add each item to the trayitems table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item =
                            JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                TrayItem tItem = new TrayItem();
                                tItem.QtyOrdered = item.Qty;
                                tItem.SellingPrice = item.MSRP;
                                if (tItem.QtyOrdered > item.QTYONHAND)
                                {
                                    tItem.QtySold = item.QTYONHAND;
                                    tItem.QtyBackOrdered = tItem.QtyOrdered - item.QTYONHAND;
                                    item.QTYONHAND = 0;
                                    item.QTYONBACKORDER = tItem.QtyBackOrdered;
                                    itemsBackOrdered = true;
                                }
                                if (tItem.QtyOrdered <= item.QTYONHAND)
                                {
                                    tItem.QtySold = tItem.QtyOrdered;
                                    item.QTYONHAND -= tItem.QtyBackOrdered;
                                }
                                tItem.ProductId = item.Id;
                                tItem.OrderId = tray.Id;
                                _db.TrayItems.Add(tItem);
                                _db.SaveChanges();
                            }
                        }
                        // test trans by uncommenting out these 3 lines
                        //int x = 1;
                        //int y = 0;
                        //x = x / y; 
                        _trans.Commit();
                        trayId = tray.Id;
                    }
                    catch (Exception ex)
                    {
                        trayId = -1;
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return trayId;
        }
    }
}