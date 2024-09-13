using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retail_v3.Models
{
    public class Products : ITableEntity
    {
        // Implement PartitionKey and RowKey
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        [Key]
        public int ProductId { get; set; }   
        public string Product_name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public int Stock_count { get; set; }
        public string Availability { get; set; }

        // Constructor to initialize the PartitionKey and RowKey
        public Products()
        {
            // Example: Partition by Category and RowKey by ProductId or a unique identifier
            PartitionKey = ProductId.ToString();
            RowKey = Guid.NewGuid().ToString();   
        }
    }
}
