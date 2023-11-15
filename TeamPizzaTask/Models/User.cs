using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamPizzaTask.Models
{
    internal class User
    {
        static uint _id = 0;
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public List<Product> Cart = new List<Product>();
        public User()
        {
            _id++;
            Id = _id;
        }


    }
}
