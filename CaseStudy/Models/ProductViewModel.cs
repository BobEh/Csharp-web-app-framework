using System.Collections.Generic;
namespace CaseStudy.Models
{
    public class ProductViewModel
    {
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string PRODUCTNAME { get; set; }
        public string GRAPHICNAME { get; set; }
        public decimal COSTPRICE { get; set; }
        public decimal MSRP { get; set; }
        public int QTYONHAND { get; set; }
        public int QTYONBACKORDER { get; set; }
    }
}