using Microsoft.VisualBasic.FileIO;
using System.Runtime.Loader;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using TeamPizzaTask.Models;
using TeamPizzaTask.Services;

namespace TeamPizzaTask
{
    internal class Program
    {
        public static User CurrentUser;

        static void Main(string[] args)
        {
            // Pizza samples 
            ProductService.AddProduct("Pizza Margherita", 20);
            ProductService.AddProduct("Pepperoni Pizza", 10);
            ProductService.AddProduct("Vegetarian Pizza", 30);

            UserService.AddUser("admin", "admin", "admin", "admin", true); // Default admin yaradilir
            try
            {
                while (true)
                {
                    Console.WriteLine("Choose from options:");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Registration");
                    Console.WriteLine("0. Exit from system\n");

                    char choice = Console.ReadKey(intercept: true).KeyChar;

                    switch (choice)
                    {
                        case '1':
                            UserService.Login();
                            break;
                        case '2':
                            UserService.Register();
                            break;
                        case '0':
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid option! Try again...\n");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UserMenu()
        {



            char choice = ' ';

            while (choice != '0')
            {
                Console.WriteLine("User menu:");
                Console.WriteLine("1. Show pizza menu");
                Console.WriteLine("2. Place your order");
                Console.WriteLine("0. Log out\n");
                choice = Console.ReadKey(intercept: true).KeyChar;
                switch (choice)
                {
                    case '1':
                        ShowPizzaMenu();
                        break;
                    case '2':
                        PlaceOrder();
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Invalid option!\n");
                        break;

                }
            }
        }

        static void ShowPizzaMenu()
        {
            Console.WriteLine("Pizza menu:");
            ProductService.GetProducts().ForEach(product => Console.WriteLine($"{product.Id}. {product.Name} Price: {product.Price} "));
            Console.Write("\nEnter ID of pizza to add to your cart: ");
            uint buyId = Convert.ToUInt32(Console.ReadLine());
        InvalidOption:
            Console.WriteLine("\nPress 'S' to add a pizza to the cart, 'G' to return to Pizza Menu , or '0' to go back to the user menu:\n");
            char key = Console.ReadKey(intercept: true).KeyChar;

            if (key == 'S' || key == 's')
            {
                if (buyId > 0 && buyId <= ProductService.GetProducts().Count)
                {
                    CurrentUser.Cart.Add(new Product
                    {
                        Id = ProductService.GetProductById(buyId).Id,
                        Name = ProductService.GetProductById(buyId).Name,
                        Price = ProductService.GetProductById(buyId).Price,
                        Quantity = 0
                    });

                    Console.WriteLine("\nPizza added to cart.");

                    Console.Write("\nEnter the quantity: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());

                    CurrentUser.Cart[CurrentUser.Cart.Count - 1].Quantity = quantity;

                    Console.WriteLine($"{quantity} pizza(s) added to cart.\n");

                    Console.WriteLine("Your Cart:");
                    CurrentUser.Cart.ForEach(cart => Console.WriteLine($"{cart.Id}. {cart.Name} Quantity: {cart.Quantity} Price: ${cart.Price * cart.Quantity}\n"));

                    ShowPizzaMenu();
                }
                else
                {
                    Console.WriteLine("Invalid pizza ID.\n");
                    ShowPizzaMenu();
                }
            }
            else if (key == 'G' || key == 'g')
            {
                ShowPizzaMenu();
            }
            else if (key == '0')
            {
                UserMenu();
            }
            else
            {
                Console.WriteLine("\nInvalid key. Please try again.\n");
                goto InvalidOption;
            }
        }



        static void PlaceOrder()
        {
            decimal totalPrice = 0;
            if (CurrentUser.Cart.Count == 0)
            {

                Console.WriteLine("Your cart is empty!\n");
                return;
            }
            Console.WriteLine("Your Cart:");
            foreach (var item in CurrentUser.Cart)
            {
                Console.WriteLine($"{item.Id}. {item.Name} Quantity: {item.Quantity} Price: ${item.Price * item.Quantity}");
                totalPrice += item.Price * item.Quantity;
            }

            Console.WriteLine($"Total Price: ${totalPrice}");

            string address;

            do
            {
                Console.Write("Enter delivery address: ");
                address = Console.ReadLine();
            } while (String.IsNullOrEmpty(address));



            string phoneNumber;
            do
            {
                Console.WriteLine("Enter phone number (e.g., +994501234567):");
                phoneNumber = Console.ReadLine();
            } while (!IsValidPhoneNumber(phoneNumber));

            Console.WriteLine($"\nOrder placed!\nTotal Price: ${totalPrice}, Address: {address}, Phone Number: {phoneNumber}\n");
            CurrentUser.Cart.Clear();
        }

        static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+994(50|51|55|70|77)\d{3}\d{2}\d{2}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneNumber);
        }

        public static void AdminMenu()
        {


            char choice = ' ';

            while (choice != '0')
            {
                Console.WriteLine("Choose what you want to do: ");
                Console.WriteLine("1. Open User Menu");
                Console.WriteLine("2. Open CRUD Menu");
                Console.WriteLine("0. Log out\n");
                choice = Console.ReadKey(intercept: true).KeyChar;
                switch (choice)
                {
                    case '1':
                        UserMenu();
                        break;
                    case '2':
                        CrudMenu();
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Invalid option!\n");
                        break;

                }
            }
        }

        static void CrudMenu()
        {

            char choice = ' ';
            while (choice != '0')
            {
                Console.WriteLine("Choose from options:");
                Console.WriteLine("1. Change Pizzas");
                Console.WriteLine("2. Change Users");
                Console.WriteLine("0. Go back\n");
                choice = Console.ReadKey(intercept: true).KeyChar;
                switch (choice)
                {
                    case '1':
                        ProductsCrud();
                        break;
                    case '2':
                        UsersCrud();
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        static void UsersCrud()
        {
            char choice = ' ';
            while (choice != '0')
            {
                Console.WriteLine("Choose from options:\n1. Show All users\n2. Add new user\n3. Update user\n4. Delete user\n0. Go back\n");
                choice = Console.ReadKey(intercept: true).KeyChar;
                switch (choice)
                {
                    case '1':
                        UserService.GetUsers().ForEach(user => Console.WriteLine($"{user.Id}. {user.Login}, Name: {user.Name}, Surname: {user.Surname}, Admin Status: {user.IsAdmin}"));
                        break;
                    case '2':
                        Console.Write("Enter username for new user: ");
                        string username = Console.ReadLine();
                        Console.Write("Enter password for new user: ");
                        string password = Console.ReadLine();
                        Console.Write("Enter name for new user: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter surname for new user: ");
                        string surname = Console.ReadLine();
                    AdminStatusCheck:
                        Console.WriteLine("New user admin status(Yes/No): ");
                        string adminStatus = Console.ReadLine().ToLower();
                        bool isAdmin;
                        if (adminStatus == "yes")
                            isAdmin = true;
                        else if (adminStatus == "no")
                            isAdmin = false;
                        else { Console.WriteLine("Invalid input"); goto AdminStatusCheck; }
                        UserService.AddUser(name, surname, username, password, isAdmin);
                        break;
                    case '3':
                        Console.Write("Enter ID of user to update: ");
                        uint idToUpdate = Convert.ToUInt32(Console.ReadLine());
                        UserService.UpdateUser(idToUpdate);
                        break;
                    case '4':
                        Console.Write("Enter ID of user to remove: ");
                        uint idToRemove = Convert.ToUInt32(Console.ReadLine());
                        UserService.RemoveUser(idToRemove);
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Invalid option!\n");
                        break;
                }
            }
        }
        static void ProductsCrud()
        {
            char choice = ' ';
            while (choice != '0')
            {
                Console.WriteLine("Choose from options:\n1. Show All pizzas\n2. Add new pizza\n3. Update pizza\n4. Delete pizza\n0. Go back\n");
                choice = Console.ReadKey(intercept: true).KeyChar;
                switch (choice)
                {
                    case '1':
                        ProductService.GetProducts().ForEach(product => Console.WriteLine($"{product.Id}. {product.Name}, Price: {product.Price}"));
                        break;
                    case '2':
                        Console.Write("Enter name of new pizza: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter price for new pizza: ");
                        decimal price = Convert.ToDecimal(Console.ReadLine());
                        ProductService.AddProduct(name, price);
                        break;
                    case '3':
                        Console.Write("Enter ID of pizza to update: ");
                        uint idToUpdate = Convert.ToUInt32(Console.ReadLine());
                        ProductService.UpdateProduct(idToUpdate);
                        break;
                    case '4':
                        Console.Write("Enter ID of pizza to remove: ");
                        uint idToRemove = Convert.ToUInt32(Console.ReadLine());
                        ProductService.RemoveProduct(idToRemove);
                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Invalid option!\n");
                        break;
                }
            }
        }
    }
}