namespace TsinghuaNet.Models
{
    public class PackageBox
    {
        public PackageBox(string name, string license)
        {
            Name = name;
            License = license;
        }

        public string Name { get; }
        public string License { get; }
    }
}
