namespace FastTelegramBot.DataTypes.InputFiles;

/// <summary>
/// Type of a <see cref="InputFile"/>
/// </summary>
public enum FileType
{
    /// <summary>
    /// FileStream
    /// </summary>
    Stream = 1,

    /// <summary>
    /// FileId
    /// </summary>
    Id,

    /// <summary>
    /// File URL
    /// </summary>
    Url
}

public static class FileTypeExtensions
{
    public static string ToJsonValue(this UpdateType updateType)
    {
        return updateType.ToString().ToLowerInvariant();
    }
}
