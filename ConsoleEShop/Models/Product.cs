namespace ConsoleEShop.Models
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        private static int nextId = 0; //Temporary solution. Id must be assigned by DB
        public Product()
        {
            Id = nextId;
            nextId++;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Price: {Price}";
        }
    }
}
