﻿namespace FastTelegramBot.DataTypes;
public readonly struct FileId
{
    public string Id { get; }

    public FileId(string fileId)
    {
        Id = fileId;
    }

    public override string ToString()
    {
        return Id;
    }
}
