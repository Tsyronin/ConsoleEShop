using ConsoleEShop.Models;
using System.Collections.Generic;

namespace ConsoleEShop
{
    public interface IRepository
    {
        //TODO: Get rid of all 'GetAll' methods
        List<Product> GetAllProducts();
        Product GetProductByName(string name);
        Product GetProductById(int prodId);
        bool AddProduct(Product product);
        bool UpdateProduct(int prodId, Product product);

        void AddOrder(Order order);
        List<Order> GetOrdersByUserId(int userId);
        Order GetOrderById(int orderId);
        bool SetNewOrderStatus(int orderId, OrderStatus status);

        User AddUser(string email, string password);
        List<User> GetAllUsers();
        User GetUserById(int userId);
        User GetUserByEmail(string email);
        bool UpdateUser(User user);
    }
}
