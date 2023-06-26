﻿using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Messages.Keyboards;
public class InlineKeyboardMarkup : IKeyboardMarkup
{
    public List<List<InlineKeyboardButton>> InlineKeyboard { get; } = new();

    public InlineKeyboardMarkup(List<List<InlineKeyboardButton>> inlineKeyboard)
    {
        InlineKeyboard = inlineKeyboard;
    }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WritePropertyName("reply_markup");
        writer.WriteStartObject();

        // keyboard
        writer.WritePropertyName("inline_keyboard");
        writer.WriteStartArray();
        foreach (var buttonsList in InlineKeyboard)
        {
            writer.WriteStartArray();
            foreach (var button in buttonsList)
            {
                button.WriteToJson(writer);
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
