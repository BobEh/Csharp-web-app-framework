using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudy.Models
{
    public class Tray
    {
        public Tray()
        {
            TrayItems = new HashSet<TrayItem>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "money")]
        public decimal OrderAmount { get; set; }
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }
        public virtual ICollection<TrayItem> TrayItems { get; set; }
    }
}
