namespace FastTelegramBot.DataTypes.InputFiles;

/// <summary>
/// This object represents a file that is already stored somewhere on the Telegram servers
/// </summary>
public class InputFileId : InputFile
{
    /// <inheritdoc/>
    public override FileType FileType => FileType.Id;

    /// <summary>
    /// A file identifier
    /// </summary>
    public FileId Id { get; private set; }

    /// <summary>
    /// This object represents a file that is already stored somewhere on the Telegram servers
    /// </summary>
    /// <param name="id">A file identifier</param>
    public InputFileId(FileId id) => Id = id;

    /// <summary>
    /// This object represents a file that is already stored somewhere on the Telegram servers
    /// </summary>
    /// <param name="id">A file identifier</param>
    public InputFileId(string id) => Id = new FileId(id);

    public override string ToString() => Id.ToString();

    public override void AddToMultipartContent(MultipartFormDataContent content, string propertyName)
    {
        content.Add(new StringContent(Id.ToString()), propertyName);
    }
}
