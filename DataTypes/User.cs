using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class User : IJsonData
{
    public long Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; }
    public string? Username { get; private set; }

    public override string ToString()
    {
        return Username is not null ? $"@{Username} (ID {Id})"
            : LastName is not null ? $"{FirstName} {LastName} (ID {Id})"
            : $"{FirstName} (ID {Id})";
    }

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
                    case "id":
                        Id = long.Parse(reader.ReadAsString());
                        break;
                    case "first_name":
                        FirstName = reader.ReadAsString() ?? "Unknown";
                        break;
                    case "last_name":
                        LastName = reader.ReadAsString();
                        break;
                    case "username":
                        Username = reader.ReadAsString();
                        break;
                }
            }
        }
    }
}
