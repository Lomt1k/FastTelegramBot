namespace FastTelegramBot.DataTypes;
public readonly struct FileUniqueId
{
    public string Id { get; }

    public FileUniqueId(string fileUniqueId)
    {
        Id = fileUniqueId;
    }

    public override string ToString()
    {
        return Id;
    }
}
