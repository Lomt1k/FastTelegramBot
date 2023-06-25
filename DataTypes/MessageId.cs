using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class MessageId : IJsonData
{
    public long Id { get; private set; }

    public override string ToString()
    {
        return Id.ToString();
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
                    case "message_id":
                        Id = long.Parse(reader.ReadAsString());
                        return;
                }
            }
        }
    }

}
