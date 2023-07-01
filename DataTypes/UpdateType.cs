namespace FastTelegramBot.DataTypes;
public enum UpdateType
{
    Unknown = 0,
    ChatMember = 1,
    MyChatMember = 2,
    Message = 3,
    EditedMessage = 4,
    ChannelPost = 5,
    EditedChannelPost = 6,
    InlineQuery = 7,
    ChosenInlineResult  = 8,
    CallbackQuery = 9,
    ShippingQuery = 10,
    PreCheckoutQuery = 11,
    Poll = 12,
    PollAnswer = 13,
    ChatJoinRequest = 14,
}

public static class UpdateTypeExtensions
{
    public static string ToJsonValue(this UpdateType updateType)
    {
        return updateType switch
        {
            UpdateType.ChatMember => "chat_member",
            UpdateType.MyChatMember => "my_chat_member",
            UpdateType.Message => "message",
            UpdateType.EditedMessage => "edited_message",
            UpdateType.ChannelPost => "channel_post",
            UpdateType.EditedChannelPost => "edited_channel_post",
            UpdateType.InlineQuery => "inline_query",
            UpdateType.ChosenInlineResult => "chosen_inline_result",
            UpdateType.CallbackQuery => "callback_query",
            UpdateType.ShippingQuery => "shipping_query",
            UpdateType.PreCheckoutQuery => "pre_checkout_query",
            UpdateType.Poll => "poll",
            UpdateType.PollAnswer => "poll_answer",
            UpdateType.ChatJoinRequest => "chat_join_request",
            _ => "unknown"
        };
    }
}
