using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace ABC_Retail_v3.Models
{
    public class Transactions : ITableEntity
    {
        // Implement PartitionKey and RowKey
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        [Key]
        public int TransactionId { get; set; }
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public double Price { get; set; }
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public int CustomerId { get; set; }
        public virtual Customers Customers { get; set; }

        public string OrderStatus { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }

        // Constructor to initialize the PartitionKey and RowKey
        public Transactions()
        {
            // Example: Partition by CustomerId and RowKey by TransactionId or a GUID
            PartitionKey = TransactionId.ToString();
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
