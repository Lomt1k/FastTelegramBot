using Newtonsoft.Json;

namespace FastTelegramBot.DataTypes.Keyboards;
public class ReplyKeyboardMarkup : IKeyboardMarkup
{
    public List<List<ReplyKeyboardButton>> Keyboard { get; } = new();
    public bool IsPersistent { get; set; }
    public bool ResizeKeyboard { get; set; }
    public bool OneTimeKeyboard { get; set; }
    public string? InputFieldPlaceholder { get; set; }

    public ReplyKeyboardMarkup(List<List<ReplyKeyboardButton>> keyboard, bool isPersistent = false, bool resizeKeyboard = false, bool oneTimeKeyboard = false, string? inputFieldPlaceholder = null)
    {
        Keyboard = keyboard;
        IsPersistent = isPersistent;
        ResizeKeyboard = resizeKeyboard;
        OneTimeKeyboard = oneTimeKeyboard;
        InputFieldPlaceholder = inputFieldPlaceholder;
    }

    public ReplyKeyboardMarkup(params string[] buttons)
    {
        var buttonsList = new List<ReplyKeyboardButton>();
        foreach (var button in buttons)
        {
            buttonsList.Add(new ReplyKeyboardButton(button));
        }
        Keyboard = new List<List<ReplyKeyboardButton>> { buttonsList };
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
