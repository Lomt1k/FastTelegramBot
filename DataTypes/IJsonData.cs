using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public interface IJsonData
{
    void ReadFromJson(JsonTextReader reader);
}
