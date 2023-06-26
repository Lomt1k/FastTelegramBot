using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Messages.Keyboards;
public class ReplyKeyboardMarkup : IKeyboardMarkup
{
    public List<List<ReplyKeyboardButton>> Keyboard { get; } = new();
    public bool IsPersistent { get; }
    public bool ResizeKeyboard { get; }
    public bool OneTimeKeyboard { get; }
    public string? InputFieldPlaceholder { get; }

    public ReplyKeyboardMarkup(List<List<ReplyKeyboardButton>> keyboard, bool isPersistent = false, bool resizeKeyboard = false, bool oneTimeKeyboard = false, string? inputFieldPlaceholder = null)
    {
        Keyboard = keyboard;
        IsPersistent = isPersistent;
        ResizeKeyboard = resizeKeyboard;
        OneTimeKeyboard = oneTimeKeyboard;
        InputFieldPlaceholder = inputFieldPlaceholder;
    }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WritePropertyName("reply_markup");
        writer.WriteStartObject();

        // keyboard
        if (Keyboard.Count > 0)
        {
            writer.WritePropertyName("keyboard");
            writer.WriteStartArray();
            foreach (var buttonsList in Keyboard)
            {
                writer.WriteStartArray();
                foreach (var button in buttonsList)
                {
                    button.WriteToJson(writer);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }

        // other fields
        if (IsPersistent)
        {
            writer.WritePropertyName("is_persistent");
            writer.WriteValue(IsPersistent);
        }
        if (ResizeKeyboard)
        {
            writer.WritePropertyName("resize_keyboard");
            writer.WriteValue(ResizeKeyboard);
        }
        if (OneTimeKeyboard)
        {
            writer.WritePropertyName("one_time_keyboard");
            writer.WriteValue(OneTimeKeyboard);
        }        
        if (InputFieldPlaceholder is not null)
        {
            writer.WritePropertyName("input_field_placeholder");
            writer.WriteValue(InputFieldPlaceholder);
        }

        writer.WriteEndObject();
    }
}
