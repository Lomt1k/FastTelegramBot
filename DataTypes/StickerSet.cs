using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTelegramBot.DataTypes;
// TODO: https://core.telegram.org/bots/api#stickerset
public class StickerSet : IJsonData
{
    public string Name { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string StickerType { get; private set; } = string.Empty;
    public bool IsAnimated { get; private set; }
    public bool IsVideo { get; private set; }
    public Sticker[] Stickers { get; private set; } = Array.Empty<Sticker>();

    public void ReadFromJson(JsonTextReader reader)
    {
        throw new NotImplementedException();
    }
}
