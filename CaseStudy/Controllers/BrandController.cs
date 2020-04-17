using Microsoft.AspNetCore.Mvc;
using CaseStudy.Models;
using System.Collections.Generic;
using CaseStudy.Utils;
using System;

namespace CaseStudy.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index(BrandViewModel vm)
        {
            // only build the catalogue once
            if (HttpContext.Session.Get<List<Brand>>("brands") == null)
            {
                // no session information so let's go to the database
                try
                {
                    BrandModel brandModel = new BrandModel(_db);
                    // now load the categories
                    List<Brand> brands = brandModel.GetAll();
                    HttpContext.Session.Set<List<Brand>>("brands", brands);
                    vm.SetBrands(brands);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Catalogue Problem - " + ex.Message;
                }
            }
            else
            {
                // no need to go back to the database as information is already in the session
                vm.SetBrands(HttpContext.Session.Get<List<Brand>>("brands"));
            }
            return View(vm);
        }
        public IActionResult SelectBrand(BrandViewModel vm)
        {
            BrandModel brandModel = new BrandModel(_db);
            ProductModel productModel = new ProductModel(_db);
            List<Product> items = productModel.GetAllByBrand(vm.BrandId);
            List<ProductViewModel> vms = new List<ProductViewModel>();
            if (items.Count > 0)
            {
                foreach (Product item in items)
                {
                    ProductViewModel mvm = new ProductViewModel();
                    mvm.Qty = 0;
                    mvm.BrandId = item.BrandId;
                    mvm.BrandName = brandModel.GetName(item.BrandId);
                    mvm.Description = item.Description;
                    mvm.Id = item.Id;
                    mvm.PRODUCTNAME = item.ProductName;
                    mvm.GRAPHICNAME = item.GraphicName;
                    mvm.COSTPRICE = Convert.ToDecimal(item.CostPrice);
                    mvm.MSRP = Convert.ToDecimal(item.MSRP);
                    mvm.QTYONHAND = item.QtyOnHand;
                    mvm.QTYONBACKORDER = item.QtyOnBackOrder;
                    vms.Add(mvm);
                }
                ProductViewModel[] myMenu = vms.ToArray();
                HttpContext.Session.Set<ProductViewModel[]>("menu", myMenu);
            }
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>("brands"));
            return View("Index", vm); // need the original Index View here
        }
        public ActionResult SelectItem(BrandViewModel vm)
        {
            Dictionary<string, object> tray;
            if (HttpContext.Session.Get<Dictionary<string, Object>>("tray") == null)
            {
                tray = new Dictionary<string, object>();
            }
            else
            {
                tray = HttpContext.Session.Get<Dictionary<string, object>>("tray");
            }
            ProductViewModel[] menu = HttpContext.Session.Get<ProductViewModel[]>("menu");
            String retMsg = "";
            foreach (ProductViewModel item in menu)
            {
                if (item.Id.Equals(vm.Id))
                {
                    if (vm.Qty > 0) // update only selected item
                    {
                        item.Qty = vm.Qty;
                        retMsg = vm.Qty + " - item(s) Added!";
                        tray[item.Id] = item;
                    }
                    else
                    {
                        item.Qty = 0;
                        tray.Remove(item.Id);
                        retMsg = "item(s) Removed!";
                    }
                    vm.BrandId = item.BrandId;
                    break;
                }
            }
            ViewBag.AddMessage = retMsg;
            HttpContext.Session.Set<Dictionary<string, Object>>("tray", tray);
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>("brands"));
            return View("Index", vm);
        }
    }

}
