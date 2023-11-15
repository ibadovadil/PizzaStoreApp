using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamPizzaTask.Models;
using TeamPizzaTask.Services;

namespace TeamPizzaTask.Databases
{
    internal static class ProductsDatabase
    {
        public static List<Product> Products { get; set; } = new List<Product>();
        public static List<Product> Cart { get; set; } = new List<Product>();


    }
}
