using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class TrayViewModel
    {
        public string TrayId { get; set; }
        public string ProductId { get; set; }
        public int QtyO { get; set; }
        public int QtyS { get; set; }
        public int QtyB { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public double Price { get; set; }
        public decimal Total { get; set; }
        public double Extended { get; set; }
        public double Tax { get; set; }
        public double Sub { get; set; }
    }
}