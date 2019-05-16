namespace TsinghuaNet.CrossPlatform.Models
{
    public enum MenuItemType
    {
        Info,
        Browse,
        Details,
        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
