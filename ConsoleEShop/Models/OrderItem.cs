namespace ConsoleEShop.Models
{
    public class OrderItem
    {
        public readonly Product product;
        public int Quantity { get; set; } = 1;
        public OrderItem(Product product)
        {
            this.product = product;
        }
        public override string ToString()
        {
            return $"Product: '{product.Name}', Quantity: {Quantity}";
        }
    }
}
