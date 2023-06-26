﻿using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Messages.Keyboards;
public class ReplyKeyboardButton : IKeyboardButton
{
    public string Text { get; } = string.Empty;
    public bool RequestContact { get; }
    public bool RequestLocation { get; }

    public ReplyKeyboardButton(string text)
    {
        Text = text;
    }

    public ReplyKeyboardButton(string text, bool requestContact = false, bool requestLocation = false)
    {
        Text = text;
        RequestContact = requestContact;
        RequestLocation = requestLocation;
    }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("text");
        writer.WriteValue(Text);
        if (RequestContact)
        {
            writer.WritePropertyName("request_contact");
            writer.WriteValue(RequestContact);
        }
        if (RequestLocation)
        {
            writer.WritePropertyName("request_location");
            writer.WriteValue(RequestLocation);
        }
        writer.WriteEndObject();
    }

    public static ReplyKeyboardButton WithRequestContact(string text)
    {
        return new ReplyKeyboardButton(text, requestContact: true);
    }

    public static ReplyKeyboardButton WithRequestLocation(string text)
    {
        return new ReplyKeyboardButton(text, requestLocation: true);
    }

}