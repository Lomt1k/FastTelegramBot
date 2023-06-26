using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Messages.Keyboards;
public interface IKeyboardMarkup
{
    void WriteToJson(JsonTextWriter writer);
}
