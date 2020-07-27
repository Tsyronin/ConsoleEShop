using ConsoleEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEShop.Menus
{
    class UserMenu : IMenu
    {
        public User User { get; }
        private protected readonly IRepository _repo;
        private protected readonly IConsole _console;

        public UserMenu(IRepository repo, IConsole console, User user)
        {
            _repo = repo;
            _console = console;
            User = user;
        }

        public bool IsActive { get; set; } = true;

        public delegate void LogOutHendler();
        public event LogOutHendler NotifyOfLoggingOut;


        private protected List<OrderItem> Cart { get; set; } = new List<OrderItem>();

        public string SeeProducts()
        {
            var products = _repo.GetAllProducts();

            return GetStringOfEnumerable<Product>(products);
        }

        private protected string GetStringOfEnumerable<T>(IEnumerable<T> enumer)
        {
            string res = String.Empty;

            foreach (T item in enumer)
            {
                res += item.ToString() + "\n";
            }

            return res;
        }

        public string FindProdByName()
        {
            _console.WriteLine("Input name of the product:");

            string name = _console.ReadLine().Trim();

            Product prod = _repo.GetProductByName(name);

            if (prod != null)
            {
                return prod.ToString();
            }
            else
            {
                return "There is no such product in the store.";
            }
        }
        public string AddToCart()
        {
            _console.WriteLine("Input the name of the product:");
            string name = _console.ReadLine();

            Product prod = _repo.GetProductByName(name);

            if (prod is null)
            {
                return "There is no such product in the store.";
            }

            if (PresentInCart(prod.Name))
            {
                OrderItem productInCart = Cart.FirstOrDefault(item => item.product.Name == prod.Name);
                productInCart.Quantity++;
                return productInCart.ToString();
            }
            else
            {
                var newItem = new OrderItem(prod);
                Cart.Add(newItem);
                return newItem.ToString();
            }
        }

        private protected bool PresentInCart(string name)
        {
            return Cart.Any(item => item.product.Name == name);
        }

        public string ShowCart()
        {
            if (Cart.Count == 0)
                Console.WriteLine("Cart is empty.");
            return GetStringOfEnumerable<OrderItem>(Cart);
        }

        public string ConfirmOrder()
        {
            if (Cart.Count == 0)
            {
                return "Your cart is empty.";
            }

            Order order = new Order()
            {
                Items = Cart,
                UserId = User.Id
            };

            if (order.TotalPrice > User.balance)
            {
                return "There is not enough money on your balance";
            }

            _repo.AddOrder(order);
            User.balance -= order.TotalPrice;
            Cart = new List<OrderItem>();
            return "You successfuly created an order. Money withdrawn." + "\n" + 
                "Please let us know as soon as you receive it.";
        }

        public string EmptyCart()
        {
            Cart = new List<OrderItem>();
            return "Your cart is empty.";
        }

        public string SeeOrderHistory()
        {
            List<Order> userOrders = _repo.GetOrdersByUserId(User.Id);

            return GetStringOfEnumerable<Order>(userOrders);
        }

        public string SetStatusReceived()
        {
            _console.WriteLine("Input ID of order you received:");
            try
            {
                int Id = Convert.ToInt32(_console.ReadLine());
                if (!_repo.GetOrdersByUserId(User.Id).Select(o => o.Id).Contains(Id))
                {
                    return "There is no order in your history with such ID.";
                }
                _repo.GetOrderById(Id).status = OrderStatus.Received;
                return $"Status 'Received' set to order with ID {Id}";
            }
            catch (OverflowException)
            {
                return "outside the range of the Int32 type.";
            }
            catch (FormatException)
            {
                return "Not valid Id!";
            }
        }

        public string CancelOrder()
        {
            _console.WriteLine("Input ID of order you want to cancel:");
            try
            {
                int Id = Convert.ToInt32(_console.ReadLine());
                if (!_repo.GetOrdersByUserId(User.Id).Select(o => o.Id).Contains(Id))
                {
                    return "There is no order in your history with such ID.";
                }
                var order = _repo.GetOrderById(Id);

                if(order.status == OrderStatus.Received ||
                    order.status == OrderStatus.Complete ||
                    order.status == OrderStatus.CanceledByUser ||
                    order.status == OrderStatus.CanceledByAdmin)
                {
                    return "You can't cancel this order";
                }

                order.status = OrderStatus.CanceledByUser;
                User.balance += order.TotalPrice;
                return $"Order with ID {Id} cancelled. Money returned";
            }
            catch (OverflowException)
            {
                return "outside the range of the Int32 type.";
            }
            catch (FormatException)
            {
                return "Not valid Id!";
            }
        }

        public string ChangePassword()
        {
            _console.WriteLine("Input your old password:");
            string pass = Console.ReadLine();

            if (pass != User.password)
            {
                return "Wrong password";
            }

            _console.WriteLine("Input your new password:");
            string newPass = _console.ReadLine();

            User.password = newPass;
            return "Password changed.";
        }

        public string CheckBalance()
        {
            return $"Your balance is {User.balance}";
        }

        public string LogOut()
        {
            NotifyOfLoggingOut?.Invoke();
            return "Logging Out...";
        }

        public string Exit()
        {
            IsActive = false;
            return "Closing...";
        }

        protected virtual Dictionary<string, Func<string>> GetOptionDict()
        {
            var dict = new Dictionary<string, Func<string>>();

            dict.Add("1", SeeProducts);
            dict.Add("2", FindProdByName);
            dict.Add("3", AddToCart);
            dict.Add("4", ShowCart);
            dict.Add("5", ConfirmOrder);
            dict.Add("6", EmptyCart);
            dict.Add("7", SeeOrderHistory);
            dict.Add("8", SetStatusReceived);
            dict.Add("9", CancelOrder);
            dict.Add("10", ChangePassword);
            dict.Add("11", CheckBalance);
            dict.Add("12", LogOut);
            dict.Add("13", Exit);

            return dict;
        }

        protected virtual void PrintOptions()
        {
            _console.WriteLine("1) See products");
            _console.WriteLine("2) Find product by name");
            _console.WriteLine("3) Add product to cart");
            _console.WriteLine("4) Show cart");
            _console.WriteLine("5) Confirm order");
            _console.WriteLine("6) Empty cart");
            _console.WriteLine("7) See order history");
            _console.WriteLine("8) Set 'Received' status");
            _console.WriteLine("9) Cancel order");
            _console.WriteLine("10) Change password");
            _console.WriteLine("11) Check balance");
            _console.WriteLine("12) Log Out");
            _console.WriteLine("13) Exit");
        }

        public string ChooseOptions()
        {
            PrintOptions();

            return ExecuteOption(GetOption());
        }

        private protected string GetOption()
        {
            return Console.ReadLine().Trim();
        }

        private protected string ExecuteOption(string opt)
        {
            Dictionary<string, Func<string>> dict = GetOptionDict();

            try
            {
                 return dict[opt]();
            }
            catch (KeyNotFoundException)
            {
                return "Incorrect input!";
            }
        }
    }
}
