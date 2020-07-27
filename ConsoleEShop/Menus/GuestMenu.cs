using ConsoleEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEShop
{
    public class GuestMenu : IMenu
    {
        private readonly IRepository _repo;
        private readonly IConsole _console;

        public GuestMenu(IRepository repo, IConsole console)
        {
            _repo = repo;
            _console = console;
        }

        public bool IsActive { get; set; } = true;

        public delegate void LogInHendler(User user);
        public event LogInHendler NotifyOfLogginIn;

        public string SeeProducts()
        {
            var products = _repo.GetAllProducts();

            return GetStringOfEnumerable<Product>(products);
        }

        private string GetStringOfEnumerable<T>(IEnumerable<T> enumer)
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

        public string LogIn()
        {
            _console.WriteLine("Email:");
            string email = _console.ReadLine();
            _console.WriteLine("Password:");
            string password = _console.ReadLine();

            User user = _repo.GetAllUsers().FirstOrDefault(u => (u.email == email) && (u.password == password));

            if (user != null)
            {
                NotifyOfLogginIn?.Invoke(user);
                return "WellCome back!";
            }
            else
            {
                return "Wrong email or password";
            }
        }

        public string Register()
        {
            _console.WriteLine("Email:");
            string email = _console.ReadLine();

            if (!email.Contains("@"))
            {
                return "Email should contain '@' symbol."; 
            }
            var registeredEmails = _repo.GetAllUsers().Select(u => u.email);
            if (registeredEmails.Contains(email))
            {
                return "Email already registered."; 
            } 

            _console.WriteLine("Password:");
            string password = _console.ReadLine();

            _console.WriteLine("Confirm password:");
            string confPass = _console.ReadLine();

            if (password != confPass)
            {
                return "Passwords don't match!"; 
            }

            User newUser = _repo.AddUser(email, password);
            NotifyOfLogginIn?.Invoke(newUser);
            return "Registered";
        }

        public string Exit()
        {
            IsActive = false;
            return "Closing...";
        }


        public string ChooseOptions()
        {
            PrintOptions();

            return ExecuteOption(GetOption());
        }

        private string ExecuteOption(string opt)
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

        private Dictionary<string, Func<string>> GetOptionDict()
        {
            var dict = new Dictionary<string, Func<string>>();

            dict.Add("1", SeeProducts);
            dict.Add("2", FindProdByName);
            dict.Add("3", LogIn);
            dict.Add("4", Register);
            dict.Add("5", Exit);

            return dict;
        }

        private string GetOption()
        {
            return _console.ReadLine().Trim();
        }

        private void PrintOptions()
        {
            _console.WriteLine("1) See products");
            _console.WriteLine("2) Find product by name");
            _console.WriteLine("3) Log In");
            _console.WriteLine("4) Register");
            _console.WriteLine("5) Exit");
        }
    }
}