using ConsoleEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEShop
{
    class SpyRepository : IRepository
    {
        private static List<User> users = new List<User>()
        {
            new User { email = "admin@test.com", password = "qwerty", IsAdmin = true },

            new User { email = "email0@test.com", password = "pass0" },
            new User { email = "email1@test.com", password = "pass1" },
            new User { email = "email2@test.com", password = "pass2" }
        };
        private static List<Product> products = new List<Product>()
        {
            new Product { Name = "prod0", Price = 0.99M },
            new Product { Name = "prod1", Price = 1.99M },
            new Product { Name = "prod2", Price = 2.99M }
        };
        private static List<Order> orders = new List<Order>()
        {
            new Order
            {
                UserId = 0,
                Items = new List<OrderItem>()
                {
                    new OrderItem(new Product { Name = "prod3", Price = 3.99M }) { Quantity = 1 },
                    new OrderItem(new Product { Name = "prod4", Price = 4.99M }) { Quantity = 2 }
                }
            },
            new Order
            {
                UserId = 1,
                Items = new List<OrderItem>()
                {
                    new OrderItem(new Product { Name = "prod5", Price = 5.99M }) { Quantity = 1 }
                }
            }

        };
        public void AddOrder(Order order)
        {
            if (!(users.Any(user => user.Id == order.UserId)))
                throw new ArgumentException("user Id not found", nameof(order));
            if (order.Items.Count == 0)
                throw new ArgumentException("items list is empty", nameof(order));

            orders.Add(order);
        }

        public bool AddProduct(Product product)
        {
            if (String.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Name shouldn't be white space or empty", nameof(product));
            if (product.Price <= 0)
                throw new ArgumentException("Price should be greater than 0", nameof(product));

            if (ProductNameAvailable(product.Name) && product.Price > 0)
            {
                products.Add(product);
                return true;
            }
            return false;
        }

        private bool ProductNameAvailable(string name)
        {
            var names = GetAllProducts().Select(p => p.Name);
            if (names.Contains(name) || String.IsNullOrWhiteSpace(name))
                return false;
            return true;
        }

        public List<Product> GetAllProducts()
        {
            return products;
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public Order GetOrderById(int orderId)
        {
            return orders.FirstOrDefault(ord => ord.Id == orderId);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return orders.Where(ord => ord.UserId == userId).ToList();
        }

        public Product GetProductById(int prodId)
        {
            return products.FirstOrDefault(prod => prod.Id == prodId);
        }

        public Product GetProductByName(string name)
        {
            return products.FirstOrDefault(prod => prod.Name == name);
        }

        public User GetUserById(int userId)
        {
            return users.FirstOrDefault(user => user.Id == userId);
        }

        public User GetUserByEmail(string email)
        {
            return users.FirstOrDefault(user => user.email == email);
        }

        public bool SetNewOrderStatus(int orderId, OrderStatus status)
        {
            Order targetOrder = orders.FirstOrDefault(ord => ord.Id == orderId);

            if (targetOrder != null)
            {
                targetOrder.status = status;
                return true;
            }
            return false;
        }

        public bool UpdateProduct(int prodId, Product product)
        {
            if (String.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Name shouldn't be white space or empty", nameof(product));
            if (product.Price <= 0)
                throw new ArgumentException("Price should be greater than 0", nameof(product));

            Product targetProduct = products.FirstOrDefault(prod => prod.Id == prodId);

            if (!ProductNameAvailable(product.Name) && (product.Name != targetProduct.Name))
                return false;

            if (targetProduct != null/* && (ProductNameAvailable(product.Name) || (product.Name == targetProduct.Name))*/)
            {
                targetProduct.Name = product.Name;
                targetProduct.Price = product.Price;
                return true;
            }
            return false;
        }

        public bool UpdateUser(User user)
        {
            if (!user.email.Contains("@"))
                throw new ArgumentException("Email should contain '@' symbol", nameof(user));
            if (String.IsNullOrWhiteSpace(user.password))
                throw new ArgumentException("Password shouldn't be white space or empty", nameof(user));

            User targetUser = users.FirstOrDefault(u => u.Id == user.Id);

            if (targetUser != null)
            {
                targetUser.email = user.email;
                targetUser.password = user.password;
                return true;
            }
            return false;
        }

        public User AddUser(string email, string password)
        {
            if (!email.Contains("@"))
                throw new ArgumentException("Email should contain '@' symbol", nameof(email));
            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password shouldn't be white space or empty", nameof(password));

            User newUser = new User()
            {
                password = password,
                email = email
            };

            users.Add(newUser);

            return newUser;
        }
    }
}
