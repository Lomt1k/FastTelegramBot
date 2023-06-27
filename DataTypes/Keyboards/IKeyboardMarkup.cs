using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public interface IKeyboardMarkup
{
    void WriteToJson(JsonTextWriter writer);
}
