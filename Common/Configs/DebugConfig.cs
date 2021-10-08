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

        [Label("Show an extra tooltip to know some extra info on items")]
        [DefaultValue(false)]
        public bool ShowExtraInfo;
    }
}
