using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
/// <summary>
/// This object represents a sticker set
/// </summary>
public class StickerSet : IJsonData
{
    /// <summary>
    /// Sticker set name
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// Sticker set title
    /// </summary>
    public string Title { get; private set; } = string.Empty;
    /// <summary>
    /// The type of the sticker is independent from its format, which is determined by the fields IsAnimated and IsVideo
    /// </summary>
    public StickerType StickerType { get; private set; }
    /// <summary>
    /// True, if the sticker set contains animated stickers
    /// </summary>
    public bool IsAnimated { get; private set; }
    /// <summary>
    /// True, if the sticker set contains video stickers
    /// </summary>
    public bool IsVideo { get; private set; }
    /// <summary>
    /// Array of all set stickers
    /// </summary>
    public Sticker[] Stickers { get; private set; } = Array.Empty<Sticker>();

    public void ReadFromJson(JsonTextReader reader)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var key = reader.Value.ToString();
                switch (key)
                {
                    case "name":
                        Name = reader.ReadAsString() ?? string.Empty;
                        break;
                    case "title":
                        Title = reader.ReadAsString() ?? string.Empty;
                        break;
                    case "sticker_type":
                        var str = reader.ReadAsString() ?? string.Empty;
                        StickerType = Enum.Parse<StickerType>(str, true);
                        break;
                    case "is_animated":
                        IsAnimated = reader.ReadAsBoolean() ?? false;
                        break;
                    case "is_video":
                        IsVideo = reader.ReadAsBoolean() ?? false;
                        break;
                    case "stickers":
                        Stickers = ReadStickersArray(reader);
                        break;
                }
            }
        }
    }

    private Sticker[] ReadStickersArray(JsonTextReader reader)
    {
        var stickersList = new List<Sticker>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray)
            {
                break;
            }
            var nextSticker = new Sticker();
            nextSticker.ReadFromJson(reader);
            stickersList.Add(nextSticker);
        }
        return stickersList.ToArray();
    }

}
