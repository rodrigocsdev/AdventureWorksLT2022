﻿namespace AdventureWorksLT2022.Models
{
    public interface IProduct
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string? Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public int? ProductCategoryID { get; set; }
        public int? ProductModelID { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }        
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class Product : IProduct
    {
        public int ProductID { get; set; }
        public required string Name { get; set; }
        public required string ProductNumber { get; set; }
        public string? Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public int? ProductCategoryID { get; set; }
        public int? ProductModelID { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }       
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
