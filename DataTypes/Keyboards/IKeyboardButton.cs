using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public interface IKeyboardButton
{
    void WriteToJson(JsonTextWriter writer);
}
