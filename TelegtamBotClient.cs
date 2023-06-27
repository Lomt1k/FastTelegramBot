using FastTelegramBot.DataTypes;
using FastTelegramBot.DataTypes.Keyboards;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var user = JsonHelper.ReadResult<User>(responseStream);
        return user;
    }

    public async Task<MessageId> SendMessageAsync(ChatId chatId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("chat_id");
                jsonWriter.WriteValue(chatId.ToString());
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
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var messageId = JsonHelper.ReadResult<MessageId>(responseStream);
        return messageId;
    }

    public async Task<bool> EditMessageTextAsync(ChatId chatId, MessageId messageId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("chat_id");
                jsonWriter.WriteValue(chatId.ToString());
                jsonWriter.WritePropertyName("message_id");
                jsonWriter.WriteValue(messageId.ToString());
                jsonWriter.WritePropertyName("text");
                jsonWriter.WriteValue(text);
                jsonWriter.WritePropertyName("parse_mode");
                jsonWriter.WriteValue(parseMode.ToString());
                if (disableWebPagePreview)
                {
                    jsonWriter.WritePropertyName("disable_web_page_preview");
                    jsonWriter.WriteValue(disableWebPagePreview);
                }
                if (inlineKeyboardMarkup is not null)
                {
                    inlineKeyboardMarkup.WriteToJson(jsonWriter);
                }
                jsonWriter.WriteEndObject();
            }
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "editMessageText", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return JsonHelper.ReadOkResult(responseStream);
    }

    public async Task<bool> EditInlineKeyboardAsync(ChatId chatId, MessageId messageId, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("chat_id");
                jsonWriter.WriteValue(chatId.ToString());
                jsonWriter.WritePropertyName("message_id");
                jsonWriter.WriteValue(messageId.ToString());
                if (inlineKeyboardMarkup is not null)
                {
                    inlineKeyboardMarkup.WriteToJson(jsonWriter);
                }
                jsonWriter.WriteEndObject();
            }
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "editMessageReplyMarkup", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return JsonHelper.ReadOkResult(responseStream);
    }

    public async Task<bool> RemoveInlineKeyboardAsync(ChatId chatId, MessageId messageId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await EditInlineKeyboardAsync(chatId, messageId, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        catch (TelegramBotException telegramBotException)
        {
            if (telegramBotException.ErrorCode == 400)
            {
                return true;
            }
            throw telegramBotException;
        }
    }

    public async Task<bool> DeleteMesageAsync(ChatId chatId, MessageId messageId, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("chat_id");
                jsonWriter.WriteValue(chatId.ToString());
                jsonWriter.WritePropertyName("message_id");
                jsonWriter.WriteValue(messageId.ToString());
                jsonWriter.WriteEndObject();
            }
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "deleteMessage", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return JsonHelper.ReadOkResult(responseStream);
    }

    public async Task<bool> AnswerCallbackQueryAsync(string callbackQueryId, string? text = null, bool showAlert = false, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("callback_query_id");
                jsonWriter.WriteValue(callbackQueryId);
                if (!string.IsNullOrEmpty(text))
                {
                    jsonWriter.WritePropertyName("text");
                    jsonWriter.WriteValue(text);
                }
                if (showAlert)
                {
                    jsonWriter.WritePropertyName("show_alert");
                    jsonWriter.WriteValue(showAlert);
                }                
                jsonWriter.WriteEndObject();
            }
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "answerCallbackQuery", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return JsonHelper.ReadOkResult(responseStream);
    }

    public async Task<MessageId> SendStickerAsync(ChatId chatId, FIleId fileId, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("chat_id");
                jsonWriter.WriteValue(chatId.ToString());
                jsonWriter.WritePropertyName("sticker");
                jsonWriter.WriteValue(fileId.ToString());
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
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "sendSticker", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var messageId = JsonHelper.ReadResult<MessageId>(responseStream);
        return messageId;
    }

    // GetStickerSetAsync IN PROGRESS!
    public async Task<StickerSet> GetStickerSetAsync(string name, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("name");
                jsonWriter.WriteValue(name);
                jsonWriter.WriteEndObject();
            }
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync(_baseRequestUrl + "getStickerSet", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var stickerSet = JsonHelper.ReadResult<StickerSet>(responseStream);
        return stickerSet;
    }


}
