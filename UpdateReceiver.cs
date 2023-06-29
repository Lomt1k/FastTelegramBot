using FastTelegramBot.DataTypes;
using Newtonsoft.Json;
using System.Text;

namespace FastTelegramBot;
internal class UpdateReceiver
{
    private readonly TelegramBotClient _botClient;
    private readonly Func<List<Update>, Task> _handleUpdatesFunc;
    private readonly int _limit;
    private readonly int _timeout;
    private readonly UpdateType[]? _allowedUpdates;
    private readonly CancellationToken _cancellationToken;

    private long _offset;

    public UpdateReceiver(TelegramBotClient botClient, Func<List<Update>, Task> handleUpdatesFunc, long offset = 0, int limit = 100, int timeout = 0, UpdateType[]? allowedUpdates = null, CancellationToken cancellationToken = default)
    {
        _botClient = botClient;
        _handleUpdatesFunc = handleUpdatesFunc;
        _offset = offset;
        _limit = limit;
        _timeout = timeout;
        _allowedUpdates = allowedUpdates;
        _cancellationToken = cancellationToken;

        Task.Run(ReceiveAcync, cancellationToken);
    }

    private async Task ReceiveAcync()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName("offset");
                    jsonWriter.WriteValue(_offset);
                    jsonWriter.WritePropertyName("limit");
                    jsonWriter.WriteValue(_limit);
                    jsonWriter.WritePropertyName("timeout");
                    jsonWriter.WriteValue(_timeout);
                    if (_allowedUpdates is not null)
                    {
                        jsonWriter.WritePropertyName("allowed_updates");
                        jsonWriter.WriteStartArray();
                        foreach (var updateType in _allowedUpdates)
                        {
                            jsonWriter.WriteValue(updateType.ToJsonValue());
                        }
                        jsonWriter.WriteEndArray();
                    }
                    jsonWriter.WriteEndObject();
                }
            }
            var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
            var httpResponse = await _botClient.HttpClient.PostAsync(_botClient.BaseRequestUrl + "getUpdates", jsonContent, _cancellationToken).ConfigureAwait(false);
            var responseStream = await httpResponse.Content.ReadAsStreamAsync(_cancellationToken).ConfigureAwait(false);

            using (var incomingUpdates = responseStream.ReadResult<IncomingUpdates>())
            {
                if (incomingUpdates.Updates.Count > 0)
                {
                    await _handleUpdatesFunc(incomingUpdates.Updates).ConfigureAwait(false);
                    _offset = incomingUpdates.LastUpdateId + 1;
                }
            }
        }
    }

}
