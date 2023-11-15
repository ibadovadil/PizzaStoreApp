namespace TeamPizzaTask.Models
{
    internal class Product
    {
        static uint _id = 0;
        public uint Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Product()
        {
            _id++;
            Id = _id;
        }
    }
}
