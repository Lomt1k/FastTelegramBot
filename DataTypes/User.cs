using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
/// <summary>
/// This object represents a Telegram user or bot
/// </summary>
public class User : IJsonData
{
    /// <summary>
    /// Unique identifier for this user or bot
    /// </summary>
    public long Id { get; private set; }
    /// <summary>
    /// User's or bot's first name
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;
    /// <summary>
    /// Optional. User's or bot's last name
    /// </summary>
    public string? LastName { get; private set; }
    /// <summary>
    /// 	Optional. User's or bot's username
    /// </summary>
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

                    default:
                        reader.Skip();
                        break;
                }
            }
        }
    }
}
