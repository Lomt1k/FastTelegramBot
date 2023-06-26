using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Messages.Keyboards;
public class InlineKeyboardButton : IKeyboardButton
{
    public string Text { get; } = string.Empty;
    public string? Url { get; }
    public string? CallbackData { get; }

    public InlineKeyboardButton(string text, string? url = null, string? callbackData = null)
    {
        Text = text;
        Url = url;
        CallbackData = callbackData;
    }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("text");
        writer.WriteValue(Text);
        if (Url is not null)
        {
            writer.WritePropertyName("url");
            writer.WriteValue(Url);
        }
        if (CallbackData is not null)
        {
            writer.WritePropertyName("callback_data");
            writer.WriteValue(CallbackData);
        }
        writer.WriteEndObject();
    }

    public static InlineKeyboardButton WithUrl(string text, string url)
    {
        return new InlineKeyboardButton(text, url: url);
    }

    public static InlineKeyboardButton WithCallbackData(string text, string callbackData)
    {
        return new InlineKeyboardButton(text, callbackData: callbackData);
    }

}
