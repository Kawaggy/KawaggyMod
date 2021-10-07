using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Localization;

namespace KawaggyMod.Core.Helpers
{
    public static partial class KawaggyHelper
    {
        public static string NewRecipeGroup(string name, string description, int[] itemList)
        {
            RecipeGroup.RegisterGroup(
                "KawaggyMod:" + name,
                new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue(description)}",
                itemList));

            return "KawaggyMod:" + name;
        }

        public static void AddDye(int item, string name, string pass)
        {
            Ref<Effect> effect = new Ref<Effect>(KawaggyMod.Instance.GetEffect("Effects/" + name));

            GameShaders.Misc[name] = new MiscShaderData(effect, pass);
            GameShaders.Armor.BindShader(item, new ArmorShaderData(effect, pass));
        }
    }
}
