﻿using FastTelegramBot.DataTypes;
using Newtonsoft.Json;

namespace FastTelegramBot;
public static class JsonExtensions
{
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

    public static void IgnoreNextObject(this JsonTextReader reader)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                return;
            }
        }
    }

}