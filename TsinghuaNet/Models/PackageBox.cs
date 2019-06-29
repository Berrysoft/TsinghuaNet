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

        public static bool operator ==(PackageBox p1, PackageBox p2) => p1.Name == p2.Name && p1.License == p2.License;
        public static bool operator !=(PackageBox p1, PackageBox p2) => !(p1 == p2);

        public override bool Equals(object obj)
            => obj is PackageBox p && this == p;

        public override int GetHashCode()
            => (Name?.GetHashCode() ?? 0) ^ (License?.GetHashCode() ?? 0);
    }
}
