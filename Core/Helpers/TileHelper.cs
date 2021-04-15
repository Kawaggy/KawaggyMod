using Microsoft.Xna.Framework;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static class TileHelper
    {
        public static Vector2 TileOffset => Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One * 12;

        public static Vector2 TileCustomPosition(int i, int j, Vector2? off = null)
        {
            return ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition - (off ?? new Vector2(0));
        }
    }
}
