using System.IO;
using System.IO.Compression;
using PlatformLink.PluginDebug;

public class BuildZipArchiving
{
    private const string FileExtension = ".zip";

    private readonly ILogger _logger;

    public BuildZipArchiving(ILogger logger)
    {
        _logger = logger;
    }

    public void Archive(string directory)
    {
        string fullPath = directory + FileExtension;

        int fileCount = 0;

        while (File.Exists(fullPath) == true)
            fullPath = $"{directory} ({++fileCount}){FileExtension}";

        ZipFile.CreateFromDirectory(directory, fullPath);
        _logger.Log($"The build was archived in zip. Full path {fullPath}");
    }
}