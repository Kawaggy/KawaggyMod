using KawaggyMod.Content.Items.Miscellaneous.Dyes;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace KawaggyMod.Core
{
    public class Shaders
    {
        public static void Load(Mod mod)
        {
            GameShaders.Misc["BurningBlood"] = new MiscShaderData(new Ref<Effect>(mod.GetEffect("Effects/BurningBlood")), "BurningBloodPass");
            GameShaders.Misc["DeathWhisper"] = new MiscShaderData(new Ref<Effect>(mod.GetEffect("Effects/DeathWhisper")), "DeathWhisperPass");

            GameShaders.Armor.BindShader(ModContent.ItemType<BurningBloodDye>(), new ArmorShaderData(new Ref<Effect>(mod.GetEffect("Effects/BurningBlood")), "BurningBloodPass"));
            GameShaders.Armor.BindShader(ModContent.ItemType<DeathWhisperDye>(), new ArmorShaderData(new Ref<Effect>(mod.GetEffect("Effects/DeathWhisper")), "DeathWhisperPass"));
        }
    }
}
