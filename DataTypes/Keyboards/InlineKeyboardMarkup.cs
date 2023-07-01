using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public class InlineKeyboardMarkup : IKeyboardMarkup
{
    public InlineKeyboardButton[][] InlineKeyboard { get; } = Array.Empty<InlineKeyboardButton[]>();

    public InlineKeyboardMarkup(InlineKeyboardButton[][] inlineKeyboard)
    {
        InlineKeyboard = inlineKeyboard;
    }

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
