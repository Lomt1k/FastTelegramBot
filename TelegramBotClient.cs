using FastTelegramBot.DataTypes;
using FastTelegramBot.DataTypes.Keyboards;
using Newtonsoft.Json;
using System.Text;

namespace FastTelegramBot;
public class TelegramBotClient
{
    public const string apiUrl = "https://api.telegram.org/";

    public HttpClient HttpClient { get; } = new();
    public string BaseRequestUrl { get; }
    public string Token { get; }

    public TelegramBotClient(string token)
    {
        BaseRequestUrl = $"{apiUrl}bot{token}/";
        Token = token;
    }

    /// <summary>
    /// A simple method for testing your bot's authentication token
    /// </summary>
    /// <returns>Basic information about the bot in form of a User object</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task<User> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var httpResponse = await HttpClient.GetAsync(BaseRequestUrl + "getMe", cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var user = responseStream.ReadResult<User>();
        return user;
    }

    /// <summary>
    /// Use this method to send text messages
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="text">Text of the message to be sent, 1-4096 characters after entities parsing</param>
    /// <param name="parseMode">Mode for parsing entities in the message text</param>
    /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
    /// <param name="disableNotification">Sends the message silently. Users will receive a notification with no sound</param>
    /// <param name="keyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <returns>Id of the sent message</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "sendMessage", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var messageId = responseStream.ReadResult<MessageId>();
        return messageId;
    }

    /// <summary>
    /// Use this method to edit text messages
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="messageId">Identifier of the message to edit</param>
    /// <param name="text">Text of the message to be sent, 1-4096 characters after entities parsing</param>
    /// <param name="parseMode">Mode for parsing entities in the message text</param>
    /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
    /// <param name="disableNotification">Sends the message silently. Users will receive a notification with no sound</param>
    /// <param name="keyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task EditMessageTextAsync(ChatId chatId, MessageId messageId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "editMessageText", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to edit only the inline keyboard markup of message
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="messageId">Identifier of the message to edit</param>
    /// <param name="inlineKeyboardMarkup">Object for an inline keyboard</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task EditInlineKeyboardAsync(ChatId chatId, MessageId messageId, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "editMessageReplyMarkup", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to remove the inline keyboard markup of message
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="messageId">Identifier of the message</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task RemoveInlineKeyboardAsync(ChatId chatId, MessageId messageId, CancellationToken cancellationToken = default)
    {
        try
        {
            await EditInlineKeyboardAsync(chatId, messageId, cancellationToken: cancellationToken).ConfigureAwait(false);
            return;
        }
        catch (TelegramBotException telegramBotException)
        {
            if (telegramBotException.ErrorCode == 400)
            {
                return;
            }
            throw telegramBotException;
        }
    }

    /// <summary>
    /// Use this method to delete a message, including service messages, with the following limitations:
    /// - A message can only be deleted if it was sent less than 48 hours ago.
    /// - Service messages about a supergroup, channel, or forum topic creation can't be deleted.
    /// - A dice message in a private chat can only be deleted if it was sent more than 24 hours ago.
    /// - Bots can delete outgoing messages in private chats, groups, and supergroups.
    /// - Bots can delete incoming messages in private chats.
    /// - Bots granted can_post_messages permissions can delete outgoing messages in channels.
    /// - If the bot is an administrator of a group, it can delete any message there.
    /// - If the bot has can_delete_messages permission in a supergroup or a channel, it can delete any message there.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="messageId">Identifier of the message to delete</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task DeleteMesageAsync(ChatId chatId, MessageId messageId, CancellationToken cancellationToken = default)
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "deleteMessage", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to send answers to callback queries sent from inline keyboards. The answer will be displayed to the user as a notification at the top of the chat screen or as an alert.
    /// </summary>
    /// <param name="callbackQueryId">Unique identifier for the query to be answered</param>
    /// <param name="text">Text of the notification. If not specified, nothing will be shown to the user, 0-200 characters</param>
    /// <param name="showAlert">If True, an alert will be shown by the client instead of a notification at the top of the chat screen</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task AnswerCallbackQueryAsync(string callbackQueryId, string? text = null, bool showAlert = false, CancellationToken cancellationToken = default)
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "answerCallbackQuery", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to send static .WEBP, animated .TGS, or video .WEBM stickers
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="fileId">FileId of the Sticker</param>
    /// <param name="disableNotification">Sends the message silently. Users will receive a notification with no sound.</param>
    /// <param name="keyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <returns>Id of the sent message</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task<MessageId> SendStickerAsync(ChatId chatId, FileId fileId, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "sendSticker", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var messageId = responseStream.ReadResult<MessageId>();
        return messageId;
    }

    /// <summary>
    /// Use this method to get a sticker set
    /// </summary>
    /// <param name="name">Name of the sticker set</param>
    /// <returns>StickerSet object</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
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
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "getStickerSet", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var stickerSet = responseStream.ReadResult<StickerSet>();
        return stickerSet;
    }

    /// <summary>
    /// Use this method to receive incoming updates using long polling
    /// </summary>
    /// <param name="handleUpdatesFunc">Function for handle incoming updates</param>
    /// <param name="offset">Identifier of the first update to be returned. Must be greater by one than the highest among the identifiers of previously received updates. By default, updates starting with the earliest unconfirmed update are returned. An update is considered confirmed as soon as getUpdates is called with an offset higher than its update_id. The negative offset can be specified to retrieve updates starting from -offset update from the end of the updates queue. All previous updates will be forgotten.</param>
    /// <param name="limit">Limits the number of updates to be retrieved. Values between 1-100 are accepted.</param>
    /// <param name="timeout">Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling. Should be positive, short polling should be used for testing purposes only.</param>
    /// <param name="allowedUpdates">List of the update types you want your bot to receive. Specify an null or empty list to receive all update types</param>
    /// <param name="cancellationToken">CancellationToken for cancel receiving</param>
    /// <returns></returns>
    public void StartPollingUpdates(Func<List<Update>, Task> handleUpdatesFunc, long offset = 0, int limit = 100, int timeout = 0, UpdateType[]? allowedUpdates = null, CancellationToken cancellationToken = default)
    {
        _ = new UpdateReceiver(this, handleUpdatesFunc, offset, limit, timeout, allowedUpdates, cancellationToken);
    }


}
