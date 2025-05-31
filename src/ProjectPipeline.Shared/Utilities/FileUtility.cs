using System.Text;

namespace ProjectPipeline.Shared.Utilities;

/// <summary>
/// Utility class for file operations
/// </summary>
public static class FileUtility
{
    /// <summary>
    /// Gets file extension from filename
    /// </summary>
    /// <param name="filename">Filename</param>
    /// <returns>File extension</returns>
    public static string GetFileExtension(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return string.Empty;

        return Path.GetExtension(filename).ToLower();
    }

    /// <summary>
    /// Validates if file is an image
    /// </summary>
    /// <param name="filename">Filename</param>
    /// <returns>True if image file</returns>
    public static bool IsImageFile(string filename)
    {
        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        var extension = GetFileExtension(filename);
        return imageExtensions.Contains(extension);
    }

    /// <summary>
    /// Validates if file is a document
    /// </summary>
    /// <param name="filename">Filename</param>
    /// <returns>True if document file</returns>
    public static bool IsDocumentFile(string filename)
    {
        var documentExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt" };
        var extension = GetFileExtension(filename);
        return documentExtensions.Contains(extension);
    }

    /// <summary>
    /// Generates a safe filename
    /// </summary>
    /// <param name="filename">Original filename</param>
    /// <returns>Safe filename</returns>
    public static string GenerateSafeFilename(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return $"file_{Guid.NewGuid():N}";

        var invalidChars = Path.GetInvalidFileNameChars();
        var safeName = new StringBuilder();

        foreach (char c in filename)
        {
            if (!invalidChars.Contains(c))
                safeName.Append(c);
            else
                safeName.Append('_');
        }

        return safeName.ToString();
    }

    /// <summary>
    /// Gets file size in human readable format
    /// </summary>
    /// <param name="bytes">File size in bytes</param>
    /// <returns>Human readable size</returns>
    public static string GetHumanReadableSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}
