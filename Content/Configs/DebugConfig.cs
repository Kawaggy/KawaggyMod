using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace KawaggyMod.Content.Configs
{
    public class DebugConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Show an extra tooltip to know custom sprite count")]
        [DefaultValue(false)]
        public bool ShowExtraSpritesCount;
    }
}
