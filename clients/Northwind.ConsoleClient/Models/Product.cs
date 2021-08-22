namespace Northwind.ConsoleClient.Models
{
    public class Product
    {
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public Supplier Supplier { get; set; }

        public Category Category { get; set; }
    }
}
