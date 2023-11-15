using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeamPizzaTask.Databases;
using TeamPizzaTask.Exceptions;
using TeamPizzaTask.Models;

namespace TeamPizzaTask.Services
{
    internal static class UserService
    {
        public static void AddUser(string name, string surname, string login, string password, bool isAdmin)
        {
            UsersDatabase.Users.Add(new User() { Name = name, Surname = surname, Login = login, Password = password, IsAdmin = isAdmin });
        }

        public static List<User> GetUsers() => UsersDatabase.Users;

        public static User GetUserById(uint id)
        {
            var user = UsersDatabase.Users.Find(b => b.Id == id);
            if (user == null)
                throw new UserNotFoundException("There is no user with this ID");
            else
                return user;
        }
        public static void UpdateUser(uint id)
        {
            var user = GetUserById(id);
            Console.WriteLine($"\nUser {user.Login} admin status: {user.IsAdmin}");
            Console.WriteLine("Choose from options:");
            Console.WriteLine("1. Give admin rights");
            Console.WriteLine("2. Take admin rights");
            char choice = Console.ReadKey(intercept: true).KeyChar;

            switch (choice)
            {
                case '1':
                    user.IsAdmin = true;
                    break;
                case '2':
                    user.IsAdmin = false;
                    break;
                default:
                    Console.WriteLine("Invalid option!\n");
                    break;
            }
        }

        public static void RemoveUser(uint id)
        {
            UsersDatabase.Users.Remove(GetUserById(id));
        }


        public static void Register()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your surname: ");
            string surname = Console.ReadLine();

            Console.Write("Enter a username (3-16 characters): ");
            string username = Console.ReadLine();
            while (!IsValidUsername(username))
            {
                Console.WriteLine("Invalid username. It must be between 3 and 16 characters.\n");
                Console.Write("Enter a valid username: ");
                username = Console.ReadLine();
            }

            Console.Write("Enter a password (6-16 characters, at least 1 uppercase, 1 lowercase, and 1 digit): ");
            string password = Console.ReadLine();
            while (!IsValidPassword(password))
            {
                Console.WriteLine("Invalid password. It must be between 6 and 16 characters and contain at least 1 uppercase letter, 1 lowercase letter, and 1 digit.\n");
                Console.Write("Enter a valid password: ");
                password = Console.ReadLine();
            }



            AddUser(name, surname, username, password, false);

            Console.WriteLine("Registration successful. You can now log in.\n");
        }

        static bool IsValidUsername(string username) => username.Length >= 3 && username.Length <= 16;

        static bool IsValidPassword(string password) => password.Length >= 6 && password.Length <= 16 && ContainsUppercase(password) && ContainsLowercase(password) && ContainsDigit(password);

        static bool ContainsUppercase(string input) => input.Any(char.IsUpper);

        static bool ContainsLowercase(string input) => input.Any(char.IsLower);

        static bool ContainsDigit(string input) => input.Any(char.IsDigit);

        public static void Login()
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            var user = Program.CurrentUser = UsersDatabase.Users.Find(u => u.Login == username && u.Password == password);

            if (user != null)
            {
                Console.WriteLine($"\nWelcome, {user.Name} {user.Surname}!\n");
                if (user.IsAdmin)
                {
                    Console.WriteLine("You are logged in as an admin.\n");
                    Program.AdminMenu();
                }
                else Program.UserMenu();
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.\n");
            }
        }



    }

}

