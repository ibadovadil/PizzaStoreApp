using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamPizzaTask.Exceptions
{
    internal class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() { }
        public ProductNotFoundException(string message) : base(message) { }
    }
}
