namespace FastTelegramBot;
public class TelegramBotException : Exception
{
    public int ErrorCode { get; }
    public string Description { get; }

    public override string Message => $"{ErrorCode}: {Description}";

    public TelegramBotException(int? errorCode, string? description)
    {
        ErrorCode = errorCode ?? 0;
        Description = description ?? string.Empty;
    }
}
