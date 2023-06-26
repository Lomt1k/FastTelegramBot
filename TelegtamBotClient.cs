using FastTelegramBot.DataTypes;
using FastTelegramBot.DataTypes.Messages;
using FastTelegramBot.DataTypes.Messages.Keyboards;
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
        var httpResponse = await _httpClient.GetAsync(_baseRequestUrl + "getMe", cancellationToken).ConfigureAwait(false);
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var user = JsonHelper.ReadResult<User>(jsonResponse);
        return user;
    }

    public async Task<MessageId> SendMessageAsync(long chatId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
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
                if (disableWebPagePreview)
                {
                    jsonWriter.WritePropertyName("disable_web_page_preview");
                    jsonWriter.WriteValue(disableWebPagePreview);
                }
                if (disableNotification)
                {
                    jsonWriter.WritePropertyName("disable_notification");
                    jsonWriter.WriteValue(disableNotification);
                }
                if (keyboardMarkup is not null)
                {
                    keyboardMarkup.WriteToJson(jsonWriter);
                }
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
