using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class UtilityModel
    {
        private AppDbContext _db;
        public UtilityModel(AppDbContext context) // will be sent by controller
        {
            _db = context;
        }

        public bool loadProductTables(string stringJson)
        {
            bool brandsLoaded = false;
            bool productsLoaded = false;
            try
            {
                dynamic objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject(stringJson);
                brandsLoaded = loadBrands(objectJson);
                productsLoaded = loadProducts(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandsLoaded && productsLoaded;
        }

        private bool loadBrands(dynamic objectJson)
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();
                List<String> allBrands = new List<String>();
                foreach (var node in objectJson)
                {
                    allBrands.Add(Convert.ToString(node["BRAND"]));
                }
                // distinct will remove duplicates before we insert them into the db
                IEnumerable<String> brands = allBrands.Distinct<String>();
                foreach (string brandname in brands)
                {
                    Brand brand = new Brand();
                    brand.Name = brandname;
                    _db.Brands.Add(brand);
                    _db.SaveChanges();
                }
                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }        private bool loadProducts(dynamic objectJson)
        {
            bool loadedProducts = false;
            try
            {
                List<Brand> brands = _db.Brands.ToList();
                // clear out the old
                _db.Products.RemoveRange(_db.Products);
                _db.SaveChanges();
                foreach (var node in objectJson)
                {
                    Product item = new Product();
                    item.Id = Convert.ToString(node["ID"].Value);
                    item.ProductName = Convert.ToString(node["PRODUCTNAME"].Value);
                    item.GraphicName = Convert.ToString(node["GRAPHICNAME"].Value);
                    item.CostPrice = Convert.ToDouble(node["COSTPRICE"].Value);
                    item.MSRP = Convert.ToDouble(node["MSRP"].Value);
                    item.QtyOnHand = Convert.ToInt32(node["QTYONHAND"].Value);
                    item.QtyOnBackOrder = Convert.ToInt32(node["QTYONBACKORDER"].Value);
                    item.Description = Convert.ToString(node["DESCRIPTION"]);
                    string brand1 = Convert.ToString(node["BRAND"].Value);
                    // add the FK here
                    foreach (Brand brand in brands)
                    {
                        if (brand.Name == brand1)
                            item.Brand = brand;
                    }
                    _db.Products.Add(item);
                    _db.SaveChanges();
                }
                loadedProducts = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedProducts;
        }
    }
}
