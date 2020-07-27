namespace ConsoleEShop.Models
{
    public class User
    {
        private static int nextId = 0; //Temporary solution. ID must be assigned by DB
        public int Id { get; }
        public string email { get; set; }
        public string password { get; set; } //Unsafe!
        public bool IsAdmin { get; set; } = false;
        public decimal balance { get; set; } = 10M;

        public User()
        {
            Id = nextId;
            nextId++;
        }

        public override string ToString()
        {
            string Role = IsAdmin ? "Admin" : "User";
            return $"Id: {Id}, Email: {email}, password: {password}, Role: {Role}";
        }
    }
}
