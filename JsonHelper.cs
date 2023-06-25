using FastTelegramBot.DataTypes;
using Newtonsoft.Json;

namespace FastTelegramBot;
public static class JsonHelper
{
    public static T ReadResult<T>(string json) where T : IJsonData, new()
    {
        var reader = new JsonTextReader(new StringReader(json));
        int? errorCode = null;
        string? description = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var key = reader.Value.ToString();
                switch (key)
                {
                    case "ok":
                        bool isOk = reader.ReadAsBoolean() ?? false;
                        if (isOk)
                        {
                            var result = new T();
                            result.ReadFromJson(reader);
                            return result;
                        }
                        break;
                    case "description":
                        description = reader.ReadAsString();
                        break;
                    case "error_code":
                        errorCode = reader.ReadAsInt32();
                        break;
                }
            }
        }
        throw new TelegramBotException(errorCode, description);
    }

}
