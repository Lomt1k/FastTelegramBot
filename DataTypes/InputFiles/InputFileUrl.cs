namespace FastTelegramBot.DataTypes.InputFiles;

/// <summary>
/// This object represents an HTTP URL for the file to be sent
/// </summary>
public class InputFileUrl : InputFile
{
    /// <inheritdoc/>
    public override FileType FileType => FileType.Url;

    /// <summary>
    /// HTTP URL for the file to be sent
    /// </summary>
    public Uri Url { get; }

    /// <summary>
    /// This object represents an HTTP URL for the file to be sent
    /// </summary>
    /// <param name="url">HTTP URL for the file to be sent</param>
    public InputFileUrl(string url) => Url = new(url);

    /// <summary>
    /// This object represents an HTTP URL for the file to be sent
    /// </summary>
    /// <param name="uri">HTTP URL for the file to be sent</param>
    public InputFileUrl(Uri uri) => Url = uri;

    public override string ToString() => Url.ToString();

    public override void AddToMultipartContent(MultipartFormDataContent content, string propertyName)
    {
        content.Add(new StringContent(Url.ToString()), propertyName);
    }
}
