using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class Message : IJsonData
{
    public int Id { get; set; }
    public User From { get; set; } = new();
    public DateTime Date { get; set; }
    public string? Text { get; set; } = string.Empty;
    public Document? Document { get; set; }

    private readonly DateTime _startUtcTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public void ReadFromJson(JsonTextReader reader)
    {
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
                    case "message_id":
                        Id = reader.ReadAsInt32() ?? 0;
                        break;
                    case "from":
                        From.ReadFromJson(reader);
                        break;
                    case "date":
                        var timestamp = reader.ReadAsDouble() ?? 0;
                        Date = _startUtcTime.AddSeconds(timestamp).ToLocalTime();
                        break;
                    case "text":
                        Text = reader.ReadAsString();
                        break;
                    case "document":
                        Document = new Document();
                        Document.ReadFromJson(reader);
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }
        }
    }

}
