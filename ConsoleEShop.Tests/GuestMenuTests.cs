using System;
using System.Collections.Generic;
using System.Text;
using ConsoleEShop;
using Xunit;
using ConsoleEShop.Menus;
using ConsoleEShop.Models;
using Moq;
using ConsoleEShop.Consoles;
using System.Linq;

namespace ConsoleEShop.Tests
{
    public class GuestMenuTests
    {
        [Fact]
        public void SeeProducts_ReturnesListOfProducts()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();
            var SampleProducts = GetSampleProducts();
            mokedRepository.Setup(funk => funk.GetAllProducts()).Returns(SampleProducts);
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper());
            string expected = SampleProducts[0].ToString() + "\n" 
                            + SampleProducts[1].ToString() + "\n";

            //Act
            string actual = guestMenu.SeeProducts();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("prod0", 0)]
        [InlineData("prod1", 1)]
        public void FindProdByName_ValidInput_ReturnesListOfProducts(string name, int num)
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();
            var SampleProducts = GetSampleProducts();

            mokedRepository.Setup(funk => funk.GetProductByName(name)).Returns(SampleProducts[num]);
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper(new List<string>() { name }));

            var expected = SampleProducts[num].ToString();

            //Act
            string actual = guestMenu.FindProdByName();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FindProdByName_InvalidInput_ReturnesListOfProducts()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();
            var SampleProducts = GetSampleProducts();

            mokedRepository.Setup(funk => funk.GetProductByName("Pseude-Input")).Returns(default(Product));
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper(new List<string>() { "Pseude-Input" }));

            var expected = "There is no such product in the store.";

            //Act
            string actual = guestMenu.FindProdByName();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LogIn_ValidInput_NotifiesSubscribers()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();
            var SampleUsers = GetSampleUsers();

            bool loggedIn = false;
            mokedRepository.Setup(funk => funk.GetAllUsers()).Returns(GetSampleUsers());
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper(new List<string>() { "email0@test.com", "pass0" }));
            guestMenu.NotifyOfLogginIn += (user) => loggedIn = true;

            //Act
            guestMenu.LogIn();

            //Assert
            Assert.True(loggedIn);
        }

        [Fact]
        public void LogIn_ValidInput_ReturnesGreeting()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();

            mokedRepository.Setup(funk => funk.GetAllUsers()).Returns(GetSampleUsers());
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper(new List<string>() { "email0@test.com", "pass0" }));

            var expected = "WellCome back!";

            //Act
            string actual = guestMenu.LogIn();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("emailWithoutAt.com", "pass", "pass", "Email should contain '@' symbol.")]
        [InlineData("email0@test.com", "pass", "pass", "Email already registered.")]
        [InlineData("emailWith@test.com", "pass1", "pass2", "Passwords don't match!")]
        public void Register_IncorrectInput_ReturnesMessage(string email, string password, string confirmPass, string message)
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();

            mokedRepository.Setup(funk => funk.GetAllUsers()).Returns(GetSampleUsers());
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper(new List<string>() { email, password, confirmPass }));

            var expected = message;

            //Act
            string actual = guestMenu.Register();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_ValidInput_NotifiesSubscribers()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();

            bool Registered = false;
            mokedRepository.Setup(funk => funk.GetAllUsers()).Returns(GetSampleUsers());
            mokedRepository.Setup(funk => funk.AddUser("valid@test.com", "pass")).Returns(new User());

            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper(new List<string>() { "valid@test.com", "pass", "pass" }));
            guestMenu.NotifyOfLogginIn += (user) => Registered = true;

            //Act
            guestMenu.Register();

            //Assert
            Assert.True(Registered);
        }

        [Fact]
        public void Exit_ReturnsMessage()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper());
            var expected = "Closing...";

            //Act
            var actual = guestMenu.Exit();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Exit_SetsIsActiveToFalse()
        {
            //Arange
            Mock<IRepository> mokedRepository = new Mock<IRepository>();
            GuestMenu guestMenu = new GuestMenu(mokedRepository.Object, new TestConsoleWrapper());

            //Act
            guestMenu.Exit();

            //Assert
            Assert.False(guestMenu.IsActive);
        }

        private List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product { Name = "prod0", Price = 0.99M },
                new Product { Name = "prod1", Price = 1.99M }
            };
        }

        private List<User> GetSampleUsers()
        {
            return new List<User>
            {
                new User { email = "email0@test.com", password = "pass0" },
                new User { email = "email1@test.com", password = "pass1" }
            };
        }
    }
}
