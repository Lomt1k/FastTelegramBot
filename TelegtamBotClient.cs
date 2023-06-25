using FastTelegramBot.DataTypes;
using Newtonsoft.Json;
using System.Text;

namespace FastTelegramBot;
public class TelegtamBotClient
{
    public const string apiUrl = "https://api.telegram.org/";

    private HttpClient _httpClient = new();
    private string _baseRequestUrl;

    public string Token { get; }

    public TelegtamBotClient(string token)
    {
        _baseRequestUrl = $"{apiUrl}bot{token}/";
        Token = token;
    }

    public async Task<User> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _baseRequestUrl + "getMe");
        var httpResponse = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var user = JsonHelper.ReadResult<User>(jsonResponse);
        return user;
    }

    public async Task<MessageId> SendMessageAsync(long chatId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, bool disableNotification = false, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("chat_id");
                jsonWriter.WriteValue(chatId);
                jsonWriter.WritePropertyName("text");
                jsonWriter.WriteValue(text);
                jsonWriter.WritePropertyName("parse_mode");
                jsonWriter.WriteValue(parseMode.ToString());
                jsonWriter.WritePropertyName("disable_web_page_preview");
                jsonWriter.WriteValue(disableWebPagePreview);
                jsonWriter.WritePropertyName("disable_notification");
                jsonWriter.WriteValue(disableNotification);
                jsonWriter.WriteEndObject();
            }
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "sendMessage", jsonContent, cancellationToken).ConfigureAwait(false);
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var messageId = JsonHelper.ReadResult<MessageId>(jsonResponse);
        return messageId;
    }




}
