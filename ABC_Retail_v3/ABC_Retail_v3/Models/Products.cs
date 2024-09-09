//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Retail_v3.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Product_id { set; get; }

        [Display(Name = "Product Name")]
        [StringLength(50)]
        public string Product_name { set; get; }

        [Display(Name = "Product Price")]
        public int Price { set; get; }

        [Display(Name = "Product Category")]
        [StringLength(50)]
        public string Category { set; get; }

        [Display(Name = "Product Ctock_count")]
        public int Stock_count { set; get; }

        [Display(Name = "Product availability")]
        [StringLength(50)]
        public string availability { set; get; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }

    }
}
