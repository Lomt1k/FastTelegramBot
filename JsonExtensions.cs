using FastTelegramBot.DataTypes;
using Newtonsoft.Json;

namespace FastTelegramBot;
public static class JsonExtensions
{
    /// <summary>
    /// Use to read object from telegram json response
    /// </summary>
    /// <typeparam name="T">IJsonData object type</typeparam>
    /// <param name="jsonStream">http response content stream</param>
    /// <exception cref="TelegramBotException"/>
    public static T ReadResult<T>(this Stream jsonStream) where T : IJsonData, new()
    {
        using (var streamReader = new StreamReader(jsonStream))
        {
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                int? errorCode = null;
                string? description = null;
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        var key = jsonReader.Value.ToString();
                        switch (key)
                        {
                            case "ok":
                                bool isOk = jsonReader.ReadAsBoolean() ?? false;
                                if (isOk)
                                {
                                    var result = new T();
                                    result.ReadFromJson(jsonReader);
                                    return result;
                                }
                                break;
                            case "description":
                                description = jsonReader.ReadAsString();
                                break;
                            case "error_code":
                                errorCode = jsonReader.ReadAsInt32();
                                break;
                        }
                    }
                }
                throw new TelegramBotException(errorCode, description);
            }
        }
    }

    /// <summary>
    /// Use to ensure OK result in telegram json response (or throw exception)
    /// </summary>
    /// <param name="jsonStream">http response content stream</param>
    /// <exception cref="TelegramBotException"/>
    public static void EnsureOkResult(this Stream jsonStream)
    {
        using (var streamReader = new StreamReader(jsonStream))
        {
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                int? errorCode = null;
                string? description = null;
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        var key = jsonReader.Value.ToString();
                        switch (key)
                        {
                            case "ok":
                                bool isOk = jsonReader.ReadAsBoolean() ?? false;
                                if (isOk)
                                {
                                    return;
                                }
                                break;
                            case "description":
                                description = jsonReader.ReadAsString();
                                break;
                            case "error_code":
                                errorCode = jsonReader.ReadAsInt32();
                                break;
                        }
                    }
                }
                throw new TelegramBotException(errorCode, description);
            }
        }
    }

}
