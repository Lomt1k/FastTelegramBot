using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public class InlineKeyboardMarkup : IKeyboardMarkup
{
    public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; } = new();

    public InlineKeyboardMarkup(List<List<InlineKeyboardButton>> inlineKeyboard)
    {
        InlineKeyboard = inlineKeyboard;
    }

    public InlineKeyboardMarkup(List<InlineKeyboardButton> inlineKeyboard)
        : this(new List<List<InlineKeyboardButton>>() { inlineKeyboard }) { }

    public InlineKeyboardMarkup(IEnumerable<InlineKeyboardButton> inlineKeyboard)
        : this(new List<List<InlineKeyboardButton>> { inlineKeyboard.ToList() }) { }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WritePropertyName("reply_markup");
        writer.WriteStartObject();

        // keyboard
        writer.WritePropertyName("inline_keyboard");
        writer.WriteStartArray();
        foreach (var buttonsList in InlineKeyboard)
        {
            writer.WriteStartArray();
            foreach (var button in buttonsList)
            {
                button.WriteToJson(writer);
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
