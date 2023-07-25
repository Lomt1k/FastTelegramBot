using FastTelegramBot.DataTypes;
using FastTelegramBot.DataTypes.InputFiles;
using FastTelegramBot.DataTypes.Keyboards;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Text;

namespace FastTelegramBot;
public class TelegramBotClient
{
    public const string apiUrl = "https://api.telegram.org/";

    public HttpClient HttpClient { get; } = new();
    public string BaseRequestUrl { get; }
    public string FileRequestUrl { get; }
    public string Token { get; }

    public TelegramBotClient(string token)
    {
        BaseRequestUrl = $"{apiUrl}bot{token}/";
        FileRequestUrl = $"{apiUrl}file/bot{token}/";
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
            using var jsonWriter = new JsonTextWriter(sw);
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
    /// <param name="inlineKeyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task EditMessageTextAsync(ChatId chatId, MessageId messageId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using var jsonWriter = new JsonTextWriter(sw);
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
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "editMessageText", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to edit captions of messages
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="messageId">Identifier of the message to edit</param>
    /// <param name="caption">New caption of the message, 0-1024 characters after entities parsing</param>
    /// <param name="parseMode">Mode for parsing entities in the message text</param>
    /// <param name="inlineKeyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task EditMessageCaptionAsync(ChatId chatId, MessageId messageId, string caption, ParseMode parseMode = ParseMode.HTML, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("chat_id");
            jsonWriter.WriteValue(chatId.ToString());
            jsonWriter.WritePropertyName("message_id");
            jsonWriter.WriteValue(messageId.ToString());
            jsonWriter.WritePropertyName("caption");
            jsonWriter.WriteValue(caption);
            jsonWriter.WritePropertyName("parse_mode");
            jsonWriter.WriteValue(parseMode.ToString());
            if (inlineKeyboardMarkup is not null)
            {
                inlineKeyboardMarkup.WriteToJson(jsonWriter);
            }
            jsonWriter.WriteEndObject();
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "editMessageCaption", jsonContent, cancellationToken).ConfigureAwait(false);
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
            using var jsonWriter = new JsonTextWriter(sw);
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
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("chat_id");
            jsonWriter.WriteValue(chatId.ToString());
            jsonWriter.WritePropertyName("message_id");
            jsonWriter.WriteValue(messageId.ToString());
            jsonWriter.WriteEndObject();
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
            using var jsonWriter = new JsonTextWriter(sw);
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
            using var jsonWriter = new JsonTextWriter(sw);
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
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(name);
            jsonWriter.WriteEndObject();
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

    /// <summary>
    /// Use this method to send general files. Bots can currently send files of any type of up to 50 MB in size.
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="document">File to send</param>
    /// <param name="caption">Document caption (may also be used when resending documents by file_id), 0-1024 characters after entities parsing</param>
    /// <param name="parseMode">Mode for parsing entities in the document caption</param>
    /// <param name="disableNotification">Sends the message silently. Users will receive a notification with no sound.</param>
    /// <param name="keyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <returns>Id of the sent message</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task<MessageId> SendDocumentAsync(ChatId chatId, InputFile document, string? caption = null, ParseMode parseMode = ParseMode.HTML, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var boundary = $"{Guid.NewGuid()}{DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture)}";
        var multipartContent = new MultipartFormDataContent(boundary)
        {
            { new StringContent(chatId.ToString()), "chat_id" }
        };
        document.AddToMultipartContent(multipartContent, "document");

        if (caption is not null)
        {
            multipartContent.Add(new StringContent(caption.ToString()), "caption");
            multipartContent.Add(new StringContent(parseMode.ToString()), "parse_mode");
        }
        if (disableNotification)
        {
            multipartContent.Add(new StringContent(disableNotification.ToString()), "disable_notification");
        }
        if (keyboardMarkup is not null)
        {
            var sb = new StringBuilder();
            var jsonWriter = new JsonTextWriter(new StringWriter(sb));
            keyboardMarkup.WriteToJson(jsonWriter);
            var jsonMarkup = sb.ToString().Replace("\"reply_markup\":", string.Empty);
            multipartContent.Add(new StringContent(jsonMarkup), "reply_markup");
        }

        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "sendDocument", multipartContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var messageId = responseStream.ReadResult<MessageId>();
        return messageId;
    }

    /// <summary>
    /// Use this method to get basic information about a file and prepare it for downloading. For the moment, bots can download files of up to 20MB in size.
    /// </summary>
    /// <param name="fileId">File identifier to get information about</param>
    /// <returns> File object</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task<DataTypes.File> GetFileAsync(FileId fileId, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("file_id");
            jsonWriter.WriteValue(fileId.ToString());
            jsonWriter.WriteEndObject();
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "getFile", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var file = responseStream.ReadResult<DataTypes.File>();
        return file;
    }

    /// <summary>
    /// Use this method to download file from telegram servers
    /// </summary>
    /// <param name="filePath">File Path</param>
    /// <param name="destination">Destination stream</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default)
    {
        var httpResponse = await HttpClient.GetAsync(FileRequestUrl + filePath, cancellationToken).ConfigureAwait(false);
        if (httpResponse.IsSuccessStatusCode)
        {
            await httpResponse.Content.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
            return;
        }
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to specify a URL and receive incoming updates via an outgoing webhook. Whenever there is an update for the bot, we will send an HTTPS POST request to the specified URL, containing a JSON-serialized Update.
    /// If you'd like to make sure that the webhook was set by you, you can specify secret data in the parameter secret_token. If specified, the request will contain a header “X-Telegram-Bot-Api-Secret-Token” with the secret token as content.
    /// </summary>
    /// <param name="url">HTTPS URL to send updates to. Use an empty string to remove webhook integration</param>
    /// <param name="certificate">Upload your public key certificate so that the root certificate in use can be checked.</param>
    /// <param name="ipAddress">The fixed IP address which will be used to send webhook requests instead of the IP address resolved through DNS</param>
    /// <param name="maxConnections">The maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery, 1-100. Defaults to 40. Use lower values to limit the load on your bot's server, and higher values to increase your bot's throughput.</param>
    /// <param name="allowedUpdates">List of the update types you want your bot to receive</param>
    /// <param name="dropPendingUpdates">Pass True to drop all pending updates</param>
    /// <param name="secretToken">A secret token to be sent in a header “X-Telegram-Bot-Api-Secret-Token” in every webhook request, 1-256 characters. Only characters A-Z, a-z, 0-9, _ and - are allowed. The header is useful to ensure that the request comes from a webhook set by you.</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task SetWebhookAsync(string url, InputFile? certificate = null, string? ipAddress = null, int maxConnections = 40, UpdateType[]? allowedUpdates = null, bool dropPendingUpdates = false, string? secretToken = null, CancellationToken cancellationToken = default)
    {
        var boundary = $"{Guid.NewGuid()}{DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture)}";
        var multipartContent = new MultipartFormDataContent(boundary)
        {
            { new StringContent(url), "url" },
            { new StringContent(maxConnections.ToString()), "max_connections" },
            { new StringContent(dropPendingUpdates.ToString()), "drop_pending_updates" },
        };

        if (certificate is not null)
        {
            certificate.AddToMultipartContent(multipartContent, "certificate");
        }
        if (ipAddress is not null)
        {
            multipartContent.Add(new StringContent(ipAddress), "ip_address");
        }
        if (secretToken is not null)
        {
            multipartContent.Add(new StringContent(secretToken), "secret_token");
        }

        if (allowedUpdates is not null)
        {
            var sb = new StringBuilder();
            var jsonWriter = new JsonTextWriter(new StringWriter(sb));
            jsonWriter.WriteStartArray();
            foreach (var update in allowedUpdates)
            {
                jsonWriter.WriteValue(update.ToJsonValue());
            }
            jsonWriter.WriteEndArray();

            multipartContent.Add(new StringContent(sb.ToString()), "allowed_updates");
        }

        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "setWebhook", multipartContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to remove webhook integration if you decide to switch back to polling updates.
    /// </summary>
    /// <param name="dropPendingUpdates">Pass True to drop all pending updates</param>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task DeleteWebhookAsync(bool dropPendingUpdates = false, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("drop_pending_updates");
            jsonWriter.WriteValue(dropPendingUpdates.ToString());
            jsonWriter.WriteEndObject();
        }
        var jsonContent = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "deleteWebhook", jsonContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        responseStream.EnsureOkResult();
    }

    /// <summary>
    /// Use this method to get current webhook status.
    /// </summary>
    /// <returns> WebhookInfo object. If the bot is using polling updates, will return an object with the url field empty.</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task<WebhookInfo> GetWebhookInfoAsync(CancellationToken cancellationToken = default)
    {
        var httpResponse = await HttpClient.GetAsync(BaseRequestUrl + "getWebhookInfo", cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return responseStream.ReadResult<WebhookInfo>();
    }

    /// <summary>
    /// Use this method to get Update object from webhook
    /// </summary>
    /// <param name="request">A request by telegram webhook</param>
    /// <param name="secretToken">A secren token to ensure that the request comes from a telegram</param>
    /// <returns>Update object or null (on error or secret token check failed)</returns>
    public static Update? GetUpdateFromWebhookListener(HttpListenerRequest request, string? secretToken = null)
    {
        if (secretToken is not null)
        {
            var token = request.Headers["X-Telegram-Bot-Api-Secret-Token"];
            if (token is null || !token.Equals(secretToken))
            {
                return null;
            }
        }

        using var streamReader = new StreamReader(request.InputStream);
        using var jsonReader = new JsonTextReader(streamReader);
        var update = new Update();
        update.ReadFromJson(jsonReader);
        return update;
    }

    /// <summary>
    /// Use this method to send photos
    /// </summary>
    /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
    /// <param name="photo">Photo to send</param>
    /// <param name="caption">Document caption (may also be used when resending documents by file_id), 0-1024 characters after entities parsing</param>
    /// <param name="parseMode">Mode for parsing entities in the document caption</param>
    /// <param name="disableNotification">Sends the message silently. Users will receive a notification with no sound.</param>
    /// <param name="keyboardMarkup">Object for an inline keyboard markup or reply keyboard markup</param>
    /// <returns>Id of the sent message</returns>
    /// <exception cref="TelegramBotException"/>
    /// <exception cref="HttpRequestException"/>
    /// <exception cref="TaskCanceledException"/>
    public async Task<MessageId> SendPhotoAsync(ChatId chatId, InputFile photo, string? caption = null, ParseMode parseMode = ParseMode.HTML, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
    {
        var boundary = $"{Guid.NewGuid()}{DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture)}";
        var multipartContent = new MultipartFormDataContent(boundary)
        {
            { new StringContent(chatId.ToString()), "chat_id" }
        };
        photo.AddToMultipartContent(multipartContent, "photo");

        if (caption is not null)
        {
            multipartContent.Add(new StringContent(caption.ToString()), "caption");
            multipartContent.Add(new StringContent(parseMode.ToString()), "parse_mode");
        }
        if (disableNotification)
        {
            multipartContent.Add(new StringContent(disableNotification.ToString()), "disable_notification");
        }
        if (keyboardMarkup is not null)
        {
            var sb = new StringBuilder();
            var jsonWriter = new JsonTextWriter(new StringWriter(sb));
            keyboardMarkup.WriteToJson(jsonWriter);
            var jsonMarkup = sb.ToString().Replace("\"reply_markup\":", string.Empty);
            multipartContent.Add(new StringContent(jsonMarkup), "reply_markup");
        }

        var httpResponse = await HttpClient.PostAsync(BaseRequestUrl + "sendPhoto", multipartContent, cancellationToken).ConfigureAwait(false);
        var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var messageId = responseStream.ReadResult<MessageId>();
        return messageId;
    }

}
