using ConsoleEShop.Consoles;
using ConsoleEShop.Menus;
using ConsoleEShop.Models;
using System;

namespace ConsoleEShop
{
    class Eshop
    {
        private User user;

        public IMenu menu = new GuestMenu(new SpyRepository(), new ConsoleWrapper());


        public void Start()
        {
            (menu as GuestMenu).NotifyOfLogginIn += LogIn;
            while (menu.IsActive)
            {
                Console.Clear();

                Console.WriteLine(menu.ChooseOptions());
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }

        public void LogIn(User user)
        {
            if (user.IsAdmin)
            {
                menu = new AdminMenu(new SpyRepository(), new ConsoleWrapper(), user);
                (menu as AdminMenu).NotifyOfLoggingOut += LogOut;
            }
            else
            {
                menu = new UserMenu(new SpyRepository(), new ConsoleWrapper(), user);
                (menu as UserMenu).NotifyOfLoggingOut += LogOut;
            }
        }

        public void LogOut()
        {
            menu = new GuestMenu(new SpyRepository(), new ConsoleWrapper());
            (menu as GuestMenu).NotifyOfLogginIn += LogIn;
        }

    }
}
