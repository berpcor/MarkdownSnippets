﻿using MarkdownSnippets;

static class HeaderWriter
{
    static string[] defaultHeaderLines;

    internal const string DefaultHeader = @"GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: {relativePath}
To change this file edit the source file and then run MarkdownSnippets.";

    static HeaderWriter() =>
        defaultHeaderLines = DefaultHeader.Lines();

    public static string WriteHeader(string relativePath, string? header, string newline)
    {
        var lines = Header(header);
        var inner = string.Join(newline, lines)
            .Replace("{relativePath}", relativePath)
            .Replace(@"\n", newline);
        return $"<!--{newline}{inner}{newline}-->{newline}";
    }

    static string[] separator = {"\r\n", "\r", "\n", @"\n"};

    static string[] Header(string? header)
    {
        if (header == null)
        {
            return defaultHeaderLines;
        }

        if (header.Contains("<!--") ||
            header.Contains("-->"))
        {
            throw new SnippetException("Header cannot contain `<!--` or `-->`.");
        }

        return header.Split(separator, StringSplitOptions.None);
    }
}