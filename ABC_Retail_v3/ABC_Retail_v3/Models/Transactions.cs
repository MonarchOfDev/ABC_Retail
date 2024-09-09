using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ABC_Retail_v3.Models
{
    public class Transactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Transaction_id { set; get; }
        // Foreign key to Products
        [ForeignKey("Cart")]
        public int CartId { set; get; }
        public virtual Cart Cart { get; set; }
        public DateTime Purchase_date { set; get; } = DateTime.Now;
        [Display(Name = "Product Price")]
        public double Price { set; get; }
        // Foreign key to Products
        [ForeignKey("Products")]
        public int Product_id { get; set; }
        public virtual Products Product { get; set; }
        // Foreign key to Users
        [ForeignKey("Users")]
        public int Customer_id { set; get; }
        public virtual Customers Customers { get; set; }
        public string Order_Status { set; get; }
    }
}
