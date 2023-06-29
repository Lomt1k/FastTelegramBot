using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
internal class IncomingUpdates : IJsonData, IDisposable
{
    public List<Update> Updates { get; private set; } = new();
    public long LastUpdateId => Updates.Last().Id;

    public void ReadFromJson(JsonTextReader reader)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray)
            {
                break;
            }
            var update = new Update();
            update.ReadFromJson(reader);
            if (update.Id != 0)
            {
                Updates.Add(update);
            }
        }
    }

    public void Dispose()
    {
        Updates.Clear();
    }
}
