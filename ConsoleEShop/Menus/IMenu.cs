namespace ConsoleEShop
{
    public interface IMenu
    {
        public string ChooseOptions();
        public bool IsActive { get; set; }
    }
}