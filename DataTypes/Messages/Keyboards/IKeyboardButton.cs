using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Messages.Keyboards;
public interface IKeyboardButton
{
    void WriteToJson(JsonTextWriter writer);
}
