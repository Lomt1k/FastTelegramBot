using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes;
public class WebhookInfo : IJsonData
{
    private readonly DateTime _startUtcTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public string Url { get; private set; } = string.Empty;
    public bool HasCustomCertificate { get; private set; }
    public int PendingUpdateCount { get; private set; }
    public string? IpAddress { get; private set; }
    public DateTime? LastErrorDate { get; private set; }
    public string? LastErrorMessage { get; private set; }
    public DateTime? LastSynchronizationErrorDate { get; private set; }
    public int? MaxConnections { get; private set; }

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
                    case "url":
                        Url = reader.ReadAsString() ?? string.Empty;
                        break;
                    case "has_custom_certificate":
                        HasCustomCertificate = reader.ReadAsBoolean() ?? false;
                        break;
                    case "pending_update_count":
                        PendingUpdateCount = reader.ReadAsInt32() ?? 0;
                        break;
                    case "ip_address":
                        IpAddress = reader.ReadAsString();
                        break;
                    case "last_error_date":
                        var errorTimestamp = reader.ReadAsDouble() ?? 0;
                        LastErrorDate = _startUtcTime.AddSeconds(errorTimestamp).ToLocalTime();
                        break;
                    case "last_error_message":
                        LastErrorMessage = reader.ReadAsString();
                        break;
                    case "last_synchronization_error_date":
                        var syncErrorTimestamp = reader.ReadAsDouble() ?? 0;
                        LastSynchronizationErrorDate = _startUtcTime.AddSeconds(syncErrorTimestamp).ToLocalTime();
                        break;
                    case "max_connections":
                        MaxConnections = reader.ReadAsInt32() ?? 0;
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }
        }
    }

}
