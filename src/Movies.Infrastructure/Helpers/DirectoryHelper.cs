namespace Movies.Infrastructure.Helpers
{
    public static class DirectoryHelper
    {
        public static string GetDirectory(string basePath, IReadOnlyDictionary<string, string> containers, string containerKey)
        {
            if (!containers.TryGetValue(containerKey, out var folder))
                throw new ArgumentException($"Container key '{containerKey}' not configured.");

            var full = Path.Combine(basePath, folder);
            Directory.CreateDirectory(full);
            return full;
        }        
    }
}
