using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace KawaggyMod.Common.Configs
{
    public class DebugConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Show an extra tooltip to know custom sprite count")]
        [DefaultValue(false)]
        public bool ShowExtraSpritesCount;
    }
}
