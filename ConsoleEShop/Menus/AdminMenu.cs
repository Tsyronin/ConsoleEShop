using ConsoleEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEShop.Menus
{
    class AdminMenu : UserMenu
    {
        public AdminMenu(IRepository repo, IConsole console, User user) : base(repo, console, user)
        {
        }

        public string SeeUsersInfo()
        {
            List<User> users = _repo.GetAllUsers();

            return GetStringOfEnumerable<User>(users);
        }

        public string ChangeUserPassword()
        {
            _console.WriteLine("Input the ID of user you want to modify:");

            int userId;
            //TODO: Create method InputId() and simplify this method
            try
            {
                userId = Convert.ToInt32(_console.ReadLine());
            }
            catch (OverflowException)
            {
                return "outside the range of the Int32 type.";
            }
            catch (FormatException)
            {
                return "Not valid Id!";
            }

            User user = _repo.GetUserById(userId);
            if (user is null)
            {
                return "No user thith such ID";
            }

            _console.WriteLine("New password:");
            var newPassword = _console.ReadLine();
            if (String.IsNullOrWhiteSpace(newPassword))
            {
                return "Invalid password!";
            }
            user.password = newPassword;

            _repo.UpdateUser(user);

            return "Password changed.";
        }

        public string AddProduct()
        {
            _console.WriteLine("Name:");
            string name = _console.ReadLine();

            _console.WriteLine("Price:");
            int price = Convert.ToInt32(_console.ReadLine());
            if (price <= 0)
            {
                return "Invalid price!";
            }

            bool success = _repo.AddProduct(new Product { Name = name, Price = price });
            if (success)
                return "Product added.";
            return "Name is invalid or already taken!";
        }


        public string ModifyProduct()
        {
            _console.WriteLine("Input ID of the product:");
            int prodId = Convert.ToInt32(_console.ReadLine());

            Product prod = _repo.GetProductById(prodId);
 
            if (prod is null)
            {
                return "No product with such ID";
            }

            Product newProd = new Product();

            _console.WriteLine("New name:");
            newProd.Name = _console.ReadLine();

            _console.WriteLine("New price:");
            newProd.Price = Convert.ToInt32(_console.ReadLine());
            if (newProd.Price <= 0)
            {
                return "Invalid price!";
            }

            bool success = _repo.UpdateProduct(prodId, newProd);
            if (success)
            {
                return "Product changed.";
            }
            return "Invaliv product data";
        }

        public string SeeOrdersOfUser()
        {
            _console.WriteLine("Input ID of the user:");
            int userId = Convert.ToInt32(_console.ReadLine());

            if (!UserExists(userId))
            {
                return "No user with such Id!";
            }

            List<Order> orders = _repo.GetOrdersByUserId(userId);

            if (orders.Count == 0)
            {
                return "User hasn't made any purchase yet.";
            }

            return GetStringOfEnumerable<Order>(orders);
        }

        private bool UserExists(int userId)
        {
            var userIds = _repo.GetAllUsers().Select(u => u.Id);
            if (userIds.Contains(userId))
                return true;
            return false;
        }

        public string ChangeOrderStatus()
        {
            _console.WriteLine("Input ID of the order:");
            int orderId = Convert.ToInt32(_console.ReadLine());

            Order order = _repo.GetOrderById(orderId);
            if (order is null)
            {
                return "No order with such ID!";
            }

            _console.WriteLine("Select status you want this order to have(New, Canceled, PaymentReceived, Sent, Complete,):");
            OrderStatus status;
            switch (_console.ReadLine().ToLower())
            {
                case "new":
                    status = OrderStatus.New;
                    break;
                case "canceled":
                    status = OrderStatus.CanceledByAdmin;
                    break;
                case "paymentreceived":
                    status = OrderStatus.PaymentReceived;
                    break;
                case "sent":
                    status = OrderStatus.Sent;
                    break;
                case "complete":
                    status = OrderStatus.Complete;
                    break;
                default:
                    return "Invalid status!";
            }
            _repo.SetNewOrderStatus(orderId, status);
            return "Status changed.";
        }

        protected override Dictionary<string, Func<string>> GetOptionDict()
        {
            var dict = new Dictionary<string, Func<string>>();

            dict.Add("1", SeeUsersInfo);
            dict.Add("2", ChangeUserPassword);
            dict.Add("3", SeeProducts);

            dict.Add("4", FindProdByName);
            dict.Add("5", AddToCart);
            dict.Add("6", ShowCart);
            dict.Add("7", ConfirmOrder);
            dict.Add("8", EmptyCart);

            dict.Add("9", AddProduct);
            dict.Add("10", ModifyProduct);
            dict.Add("11", SeeOrdersOfUser);
            dict.Add("12", ChangeOrderStatus);
            dict.Add("13", LogOut);
            dict.Add("14", Exit);

            return dict;
        }

        protected override void PrintOptions()
        {
            _console.WriteLine("1) See users info");
            _console.WriteLine("2) Change user's password");
            _console.WriteLine("3) See products");

            _console.WriteLine("4) Find product by name");
            _console.WriteLine("5) Add product to cart");
            _console.WriteLine("6) Show cart");
            _console.WriteLine("7) Confirm order");
            _console.WriteLine("8) Empty cart");


            _console.WriteLine("9) Add product");
            _console.WriteLine("10) Modify product");
            _console.WriteLine("11) See orders of user");
            _console.WriteLine("12) Change order status");
            _console.WriteLine("13) Log Out");
            _console.WriteLine("14) Exit");
        }
    }
}

