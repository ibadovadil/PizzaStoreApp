using TeamPizzaTask.Databases;
using TeamPizzaTask.Exceptions;
using TeamPizzaTask.Models;

namespace TeamPizzaTask.Services
{
    internal static class ProductService
    {
        public static void AddProduct(string name, decimal price)
        {
            ProductsDatabase.Products.Add(new Product { Name = name, Price = price });
        }

        public static List<Product> GetProducts()
        {
            return ProductsDatabase.Products;
        }

        public static Product GetProductById(uint id)
        {
            var product = ProductsDatabase.Products.Find(b => b.Id == id);
            if (product == null)
                throw new ProductNotFoundException("There is no product with this ID");
            else
                return product;
        }
        public static void UpdateProduct(uint id)
        {
            var product = GetProductById(id);
            Console.WriteLine("\nWhat do you want to update?");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Price");
            char choice = Console.ReadKey(intercept: true).KeyChar;

            switch (choice)
            {
                case '1':
                    Console.Write("\nEnter new name: ");
                    product.Name = Console.ReadLine();
                    break;
                case '2':
                    Console.Write("\nEnter new price: ");
                    product.Price = Convert.ToDecimal(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Invalid option!\n");
                    break;
            }
        }

        public static void RemoveProduct(uint id)
        {
            ProductsDatabase.Products.Remove(GetProductById(id));
        }
    }
}
