using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Retail_v3.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        // Foreign key to Customers
        [ForeignKey("Customers")]
        public int Customer_id { set; get; }
        public virtual Customers? Customers { get; set; }

        // Foreign key to Products
        [ForeignKey("Products")]
        public int Product_id { get; set; }
        public virtual Products Products { get; set; }

        public int Quantity { get; set; }

        public DateTime Addition_Date { get; set; } = DateTime.Now;
        public ICollection<Transactions>? Transactions { get; set; }
    }
}
