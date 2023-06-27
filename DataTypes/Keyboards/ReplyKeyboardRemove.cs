using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public class ReplyKeyboardRemove : IKeyboardMarkup
{
    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WritePropertyName("reply_markup");
        writer.WriteStartObject();
        writer.WritePropertyName("remove_keyboard");
        writer.WriteValue(true);
        writer.WriteEndObject();
    }
}
