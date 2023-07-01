using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public class ReplyKeyboardButton : IKeyboardButton
{
    public string Text { get; set; } = string.Empty;
    public bool RequestContact { get; set; }
    public bool RequestLocation { get; set; }

    public ReplyKeyboardButton(string text)
    {
        Text = text;
    }

    public ReplyKeyboardButton(string text, bool requestContact = false, bool requestLocation = false)
    {
        Text = text;
        RequestContact = requestContact;
        RequestLocation = requestLocation;
    }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("text");
        writer.WriteValue(Text);
        if (RequestContact)
        {
            writer.WritePropertyName("request_contact");
            writer.WriteValue(RequestContact);
        }
        if (RequestLocation)
        {
            writer.WritePropertyName("request_location");
            writer.WriteValue(RequestLocation);
        }
        writer.WriteEndObject();
    }

    public static ReplyKeyboardButton WithRequestContact(string text)
    {
        return new ReplyKeyboardButton(text, requestContact: true);
    }

    public static ReplyKeyboardButton WithRequestLocation(string text)
    {
        return new ReplyKeyboardButton(text, requestLocation: true);
    }

    public static implicit operator ReplyKeyboardButton(string text) => new(text);

}