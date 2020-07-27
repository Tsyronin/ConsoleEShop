using System.Collections.Generic;
using System.Linq;

namespace ConsoleEShop.Models
{
    public class Order
    {
        private static int nextId = 0; //Temporary solution. Id must be assigned by DB
        public int Id { get; }
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public OrderStatus status = OrderStatus.New;

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.product.Price * item.Quantity);
            }
        }
        public Order()
        {
            Id = nextId;
            nextId++;
        }

        public override string ToString()
        {
            string res = $"Order ID: {Id}" + "\n";
            foreach (OrderItem item in Items)
                res += $"{item.product.Name} x{item.Quantity}" + "\n";
            res += $"Total Price: {TotalPrice}" + "\n";
            res += $"Status: {status}" + "\n";
            return res;
        }
    }
    public enum OrderStatus
    {
        New,
        CanceledByAdmin,
        PaymentReceived,
        Sent,
        Received,
        Complete,
        CanceledByUser
    }
}
