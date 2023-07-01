namespace FastTelegramBot.DataTypes;
public struct ChatId
{
    public long? Id;
    public string? Username;

    public ChatId(long id)
    {
        Id = id;
    }

    public ChatId(string username)
    {
        Username = username;
    }

    public override string ToString()
    {
        return Id?.ToString() ?? Username?.ToString() ?? string.Empty;
    }

    public static implicit operator ChatId(long id) => new(id);
    public static implicit operator ChatId(string username) => new(username);
}
