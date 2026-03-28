namespace Movies.Application.Contracts.Messaging
{
    public abstract class FileMessage
    {
        public FileAction Action { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string ContainerName { get; set; } = string.Empty;

        public string? ExistingFilePath { get; set; }

    }
}
