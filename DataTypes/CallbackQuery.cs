using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class CallbackQuery : IJsonData
{
    public string Id { get; private set; } = string.Empty;
    public User From { get; private set; } = new();
    public string? Data { get; private set; } = string.Empty;

    public void ReadFromJson(JsonTextReader reader)
    {
        bool isFromReaded = false;
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                return;
            }
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var key = reader.Value.ToString();
                switch (key)
                {
                    case "id":
                        if (string.IsNullOrEmpty(Id))
                        {
                            Id = reader.ReadAsString() ?? string.Empty;
                        }
                        break;
                    case "from":
                        if (!isFromReaded)
                        {
                            From.ReadFromJson(reader);
                            isFromReaded = true;
                        }
                        break;
                    case "data":
                        if (string.IsNullOrEmpty(Data))
                        {
                            Data = reader.ReadAsString();
                        }
                        break;

                    default:
                        reader.Read();
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            reader.IgnoreCurrentObject();
                        }
                        break;
                }
            }
        }
    }

}
