using System;

namespace Core {
    public class Product {

        public Product (Guid id, string description, DateTime createdAt) {
            this.Id = id;
            this.Description = description;
            this.CreatedAt = createdAt;
        }
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public static Product CreateNewProduct(string description) => new Product(Guid.NewGuid(),description,DateTime.Now);

       public override string ToString() 
       {
           return  $"Product {Description} created at {CreatedAt.ToShortDateString()}";
       }
    }
}