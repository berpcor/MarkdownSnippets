using System;
using System.Collections.Generic;
using System.IO;
using MAB.DotIgnore;
using MarkdownSnippets;

class CachedDirectoryInclude
{
    DirectoryInclude directoryInclude;
    Dictionary<string, bool> cache = new(StringComparer.OrdinalIgnoreCase);

    public CachedDirectoryInclude(DirectoryInclude? directoryInclude, string targetDirectory)
    {
        directoryInclude ??= _ => true;
        var gitIgnorePath = Path.Combine(targetDirectory, ".gitignore");
        if (File.Exists(gitIgnorePath))
        {
            var ignores = new IgnoreList(gitIgnorePath);
            this.directoryInclude = path => directoryInclude(path) &&
                                            !DirectoryExclusions.ShouldExcludeDirectory(path) &&
                                            !ignores.IsIgnored(path, true);
        }
        else
        {
            this.directoryInclude = path => directoryInclude(path) &&
                                            !DirectoryExclusions.ShouldExcludeDirectory(path);
        }
    }

    public bool Include(string path)
    {
        if (cache.TryGetValue(path, out var value))
        {
            return value;
        }

        return cache[path] = directoryInclude(path);
    }
}