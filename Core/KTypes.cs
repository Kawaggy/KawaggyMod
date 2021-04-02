using Terraria.ModLoader;

namespace KawaggyMod.Core // ugly to be all in one file but it makes sense in this case.
{
    public abstract class KItem : ModItem
    {
        public override string Texture
        {
            get
            {
                KawaggyMod.Instance.Logger.Warn($"No texture found for Item: {Name}");
                return Assets.NoTexture;
            }
        }
    }

    public abstract class KProjectile : ModProjectile
    {
        public override string Texture
        {
            get
            {
                KawaggyMod.Instance.Logger.Warn($"No texture found for Projectile: {Name}");
                return Assets.NoTexture;
            }
        }
    }

    public abstract class KNPC : ModNPC
    {
        public override string Texture
        {
            get
            {
                KawaggyMod.Instance.Logger.Warn($"No texture found for NPC: {Name}");
                return Assets.NoTexture;
            }
        }
    }

    public abstract class KBuff : ModBuff
    {
        /// <summary>
        /// The file name of this Buff's texture file in the mod loader's file space.
        /// </summary>
        public virtual string Texture
        {
            get
            {
                return Assets.NoTexture;
            }
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = Texture;
            return base.Autoload(ref name, ref texture);
        }
    }

    public abstract class KTile : ModTile
    {
        /// <summary>
        /// The file name of this Tile's texture file in the mod loader's file space.
        /// </summary>
        public virtual string Texture
        {
            get
            {
                return Assets.NoTexture + "_Tile";
            }
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = Texture;
            return base.Autoload(ref name, ref texture);
        }
    }
}
