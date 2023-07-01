# FastTelegramBot

## How this project came about:

Initially, I started using Telegram.Bot API for my personal project. I soon noticed that JSON serialization and deserialization could be greatly optimized.

What the normal work of the bot looks like:
1. Your bot receives updates from telegram servers in JSON format.
2. You need to deserialize this JSON into an update object.
3. The internal logic of your bot works, as a result of which, in most cases, you will need to send a response to the incoming update. Usually a message.
4. You serialize your response into a JSON and send it to telegram servers.
5. Telegram sends another JSON to your request, which contains all the information about the message you sent.
6. You again deserialize the incoming JSON into a message object (although in most cases you would only need to get the id of the sent message).

Each JSON serialization and deserialization occurs with the help of Telegram.Bot using reflection. At the same time, all fields from the incoming JSON are completely deserialized.

This project aims to significantly optimize the serialization and deserialization of objects, which can significantly reduce the load on RAM and CPU.

## Due to what:
1. Manual JSON serialization without using reflection, filling only the really necessary fields.
2. Manual JSON deserialization without using reflection, with reading only really necessary fields.

## Important:
Implemented only those methods and functions that I needed to work on a real project. If you found this repository and are interested in it, then you can always expand this functionality if you wish.

## Supported UpdateTypes:
- Message
- CallbackQuery

### Simple usage example:
```
using FastTelegramBot;
using FastTelegramBot.DataTypes;

public class Program
{
    private static TelegramBotClient botClient = new TelegramBotClient("YOUR_TOKEN");

    private static async Task Main(string[] args)
    {
        botClient.StartPollingUpdates(HandleUpdates, allowedUpdates: new UpdateType[] { UpdateType.Message, UpdateType.CallbackQuery } );
        Console.ReadLine();
    }

    private static Task HandleUpdates(List<Update> updates)
    {
        foreach (var update in updates)
        {
            Task.Run(() => HandleUpdate(update));
        }
        return Task.CompletedTask;
    }

    private static async Task HandleUpdate(Update update)
    {
        switch (update.UpdateType)
        {
            case UpdateType.Message:
                var text = update.Message.Text;
                if (text is not null)
                {
                    await botClient.SendMessageAsync(update.Message.From.Id, $"Your message:\n{text}");
                }
                break;
            default:
                Console.WriteLine("Unsupported updateType: " + update.UpdateType);
                break;
        }
    }
}
```

### Available Methods

**Check authorization:**
- Task\<User\> GetMeAsync(CancellationToken cancellationToken = default)

**Getting updates:**
- void StartPollingUpdates(Func<List<Update>, Task> handleUpdatesFunc, long offset = 0, int limit = 100, int timeout = 0, UpdateType[]? allowedUpdates = null, CancellationToken cancellationToken = default)
- Task SetWebhookAsync(string url, InputFile? certificate = null, string? ipAddress = null, int maxConnections = 40, UpdateType[]? allowedUpdates = null, bool dropPendingUpdates = false, string? secretToken = null, CancellationToken cancellationToken = default)
- Task DeleteWebhookAsync(bool dropPendingUpdates = false, CancellationToken cancellationToken = default)
- Task\<WebhookInfo\> GetWebhookInfoAsync(CancellationToken cancellationToken = default)
- static Update? GetUpdateFromWebhookListener(HttpListenerRequest request, string? secretToken = null)

**Messages:**
- Task\<MessageId\> SendMessageAsync(ChatId chatId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
- Task EditMessageTextAsync(ChatId chatId, MessageId messageId, string text, ParseMode parseMode = ParseMode.HTML, bool disableWebPagePreview = false, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
- Task EditInlineKeyboardAsync(ChatId chatId, MessageId messageId, InlineKeyboardMarkup? inlineKeyboardMarkup = null, CancellationToken cancellationToken = default)
- Task RemoveInlineKeyboardAsync(ChatId chatId, MessageId messageId, CancellationToken cancellationToken = default)
- Task DeleteMesageAsync(ChatId chatId, MessageId messageId, CancellationToken cancellationToken = default)
- Task AnswerCallbackQueryAsync(string callbackQueryId, string? text = null, bool showAlert = false, CancellationToken cancellationToken = default)

**Stickers:**
- Task\<MessageId\> SendStickerAsync(ChatId chatId, FileId fileId, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
- Task\<StickerSet\> GetStickerSetAsync(string name, CancellationToken cancellationToken = default)

**Files:**
- Task\<MessageId\> SendDocumentAsync(ChatId chatId, InputFile document, string? caption = null, ParseMode parseMode = ParseMode.HTML, bool disableNotification = false, IKeyboardMarkup? keyboardMarkup = null, CancellationToken cancellationToken = default)
- Task\<File\> GetFileAsync(FileId fileId, CancellationToken cancellationToken = default)
- Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default)
