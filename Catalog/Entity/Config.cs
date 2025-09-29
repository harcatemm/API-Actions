namespace Catalog.Entity
{
    public class Config
    {
        public AppSettings AppSettings { get; set; } = null!;
        public ConnectionStrings ConnectionStrings { get; set; } = null!;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }
    public class AppSettings
    {
        public string Path { get; set; } = string.Empty;        
    }
}
