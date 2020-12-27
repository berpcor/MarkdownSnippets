using System;

namespace MarkdownSnippets
{
    public delegate bool DirectoryFilter(string directoryPath);

    public class PathExclusions
    {
        public PathExclusions(
            Func<string, bool> shouldExcludeFile,
            Func<string, bool> shouldExcludeDirectory)
        {
            ShouldExcludeFile = shouldExcludeFile;
            ShouldExcludeDirectory = shouldExcludeDirectory;
        }

        public Func<string,bool> ShouldExcludeFile { get; }
        public Func<string,bool> ShouldExcludeDirectory{ get; }
    }
}