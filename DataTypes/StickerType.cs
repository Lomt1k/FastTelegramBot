namespace FastTelegramBot.DataTypes;
/// <summary>
/// The type of the sticker is independent from its format, which is determined by the fields IsAnimated and IsVideo
/// </summary>
public enum StickerType
{
    Regular,
    Mask,
    Custom_Emoji,
}
