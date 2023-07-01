using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public class ReplyKeyboardMarkup : IKeyboardMarkup
{
    public ReplyKeyboardButton[][] Keyboard { get; } = Array.Empty<ReplyKeyboardButton[]>();
    public bool IsPersistent { get; set; }
    public bool ResizeKeyboard { get; set; }
    public bool OneTimeKeyboard { get; set; }
    public string? InputFieldPlaceholder { get; set; }

    public ReplyKeyboardMarkup(ReplyKeyboardButton[][] keyboard, bool isPersistent = false, bool resizeKeyboard = false, bool oneTimeKeyboard = false, string? inputFieldPlaceholder = null)
    {
        Keyboard = keyboard;
        IsPersistent = isPersistent;
        ResizeKeyboard = resizeKeyboard;
        OneTimeKeyboard = oneTimeKeyboard;
        InputFieldPlaceholder = inputFieldPlaceholder;
    }

    public ReplyKeyboardMarkup(params string[] buttons)
    {
        var buttonsArray = new ReplyKeyboardButton[buttons.Length];
        for (var i = 0; i < buttonsArray.Length; i++)
        {
            buttonsArray[i] = new ReplyKeyboardButton(buttons[i]);
        }
        Keyboard = new ReplyKeyboardButton[][] { buttonsArray };
    }

    public void WriteToJson(JsonTextWriter writer)
    {
        writer.WritePropertyName("reply_markup");
        writer.WriteStartObject();

        // keyboard
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
