using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class Update : IJsonData
{
    public UpdateType UpdateType { get; private set; } = UpdateType.Unknown;
    public int Id { get; private set; }
    public Message? Message { get; private set; }
    public CallbackQuery? CallbackQuery { get; private set; }

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
                    case "update_id":
                        Id = reader.ReadAsInt32() ?? 0;
                        break;
                    case "message":
                        UpdateType = UpdateType.Message;
                        Message = new Message();
                        Message.ReadFromJson(reader);
                        break;
                    case "callback_query":
                        UpdateType = UpdateType.CallbackQuery;
                        CallbackQuery = new CallbackQuery();
                        CallbackQuery.ReadFromJson(reader);
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }
        }
    }

}
