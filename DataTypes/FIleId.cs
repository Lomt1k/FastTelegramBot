namespace FastTelegramBot.DataTypes;
public struct FIleId
{
    public string Id { get; }

    public FIleId(string fileId)
    {
        Id = fileId;
    }

    public override string ToString()
    {
        return Id;
    }
}
