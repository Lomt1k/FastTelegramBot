using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class File : IJsonData
{
    public FileId FileId { get; private set; }
    public FileUniqueId FileUniqueId { get; private set; }
    public long? FileSize { get; private set; }
    public string? FilePath { get; private set; } = string.Empty;


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
                    case "file_id":
                        FileId = new FileId(reader.ReadAsString() ?? string.Empty);
                        break;
                    case "file_unique_id":
                        FileUniqueId = new FileUniqueId(reader.ReadAsString() ?? string.Empty);
                        break;
                    case "file_size":
                        FileSize = long.Parse(reader.ReadAsString() ?? "-1");
                        break;
                    case "file_path":
                        FilePath = reader.ReadAsString();
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }
        }
    }
}
