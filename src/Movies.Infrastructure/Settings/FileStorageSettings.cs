namespace Movies.Infrastructure.Settings
{
    public class FileStorageSettings
    {
        public string ConnectionString { get; set; } = null!;
        public Dictionary<string, string> Containers { get; set; } = new Dictionary<string, string>();
        public string LocalStoragePath { get; set; } = null!;
        public string Provider { get; set; } = null!;
    }
}
