using System.Collections.Generic;
using System.IO;
using System.Linq;
using MarkdownSnippets;

class IncludeFileFinder
{
    DirectoryInclude directoryInclude;

    public IncludeFileFinder(DirectoryInclude directoryInclude)
    {
        this.directoryInclude = directoryInclude;
    }

    bool IncludeDirectory(string directoryPath)
    {
        if (DirectoryExclusions.ShouldExcludeDirectory(directoryPath))
        {
            return false;
        }

        return directoryInclude(directoryPath);
    }

    static bool IncludeFile(string path)
    {
        var fileName = Path.GetFileName(path);
        if (fileName.StartsWith("."))
        {
            return false;
        }

        return fileName.EndsWith(".include.md");
    }

    public List<string> FindFiles(params string[] directoryPaths)
    {
        Guard.AgainstNull(directoryPaths, nameof(directoryPaths));
        List<string> files = new();
        foreach (var directoryPath in directoryPaths)
        {
            Guard.DirectoryExists(directoryPath, nameof(directoryPath));
            FindFiles(Path.GetFullPath(directoryPath), files);
        }

        return files;
    }

    void FindFiles(string directoryPath, List<string> files)
    {
        foreach (var file in Directory.EnumerateFiles(directoryPath)
            .Where(IncludeFile))
        {
            files.Add(file);
        }

        foreach (var subDirectory in Directory.EnumerateDirectories(directoryPath)
            .Where(IncludeDirectory))
        {
            FindFiles(subDirectory, files);
        }
    }
}