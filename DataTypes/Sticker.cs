using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
/// <summary>
/// This object represents a sticker
/// </summary>
public class Sticker : IJsonData
{
    /// <summary>
    /// Identifier for this file, which can be used to download or reuse the file
    /// </summary>
    public FileId FileId { get; private set; }
    /// <summary>
    /// Unique identifier for this file, which is supposed to be the same over time and for different bots. Can't be used to download or reuse the file.
    /// </summary>
    public FileUniqueId FileUniqueId { get; private set; }
    /// <summary>
    /// The type of the sticker is independent from its format, which is determined by the fields IsAnimated and IsVideo
    /// </summary>
    public StickerType Type { get; private set; }
    /// <summary>
    /// Sticker width
    /// </summary>
    public int Width { get; private set; }
    /// <summary>
    /// Sticker height
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// True, if the sticker is animated
    /// </summary>
    public bool IsAnimated { get; private set; }
    /// <summary>
    /// True, if the sticker is a video sticker
    /// </summary>
    public bool IsVideo { get; private set; }
    /// <summary>
    /// Optional. Emoji associated with the sticker
    /// </summary>
    public string? Emoji { get; private set; }
    /// <summary>
    /// Optional. Name of the sticker set to which the sticker belongs
    /// </summary>
    public string? SetName { get; private set; }
    /// <summary>
    /// Optional. File size in bytes
    /// </summary>
    public int? FileSize { get; private set; }



    public void ReadFromJson(JsonTextReader reader)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                return;
            }
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.IgnoreNextObject();
                continue;
            }
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var key = reader.Value.ToString();
                switch (key)
                {
                    case "file_id":
                        FileId = new FileId(reader.ReadAsString() ?? string.Empty);
                        break;
                    case "file_unique_id":
                        FileUniqueId = new FileUniqueId(reader.ReadAsString() ?? string.Empty);
                        break;
                    case "type":
                        var str = reader.ReadAsString() ?? string.Empty;
                        Type = Enum.Parse<StickerType>(str, true);
                        break;
                    case "width":
                        Width = reader.ReadAsInt32() ?? 0;
                        break;
                    case "height":
                        Height = reader.ReadAsInt32() ?? 0;
                        break;
                    case "is_animated":
                        IsAnimated = reader.ReadAsBoolean() ?? false;
                        break;
                    case "is_video":
                        IsAnimated = reader.ReadAsBoolean() ?? false;
                        break;
                    case "emoji":
                        Emoji = reader.ReadAsString();
                        break;
                    case "set_name":
                        SetName = reader.ReadAsString();
                        break;
                    case "file_size":
                        FileSize = reader.ReadAsInt32();
                        break;
                }
            }
        }
    }

}
