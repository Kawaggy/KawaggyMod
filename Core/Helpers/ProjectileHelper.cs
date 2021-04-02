using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static class ProjectileHelper
    {
        public static bool DrawHook(this Projectile projectile, Vector2 center, SpriteBatch spriteBatch, string texturePath)
        {
            Vector2 distToProj = projectile.Center;
            float projRotation = projectile.AngleTo(center) - MathHelper.PiOver2;
            bool doIDraw = true;
            Texture2D texture = KawaggyMod.Instance.GetTexture(texturePath);

            while (doIDraw)
            {
                float distance = (center - distToProj).Length();
                if (distance < (texture.Height + 1))
                {
                    doIDraw = false;
                }
                else if (!float.IsNaN(distance))
                {
                    Color drawColor = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                    distToProj += projectile.DirectionTo(center) * texture.Height;
                    spriteBatch.Draw(
                        texture,
                        distToProj - Main.screenPosition,
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        drawColor,
                        projRotation,
                        texture.Size() / 2f,
                        1f,
                        SpriteEffects.None,
                        0f);
                }

            }
            return false;
        }

        public static int TargetClosestEnemy(this Projectile projectile, bool needsCanBeChasedBy = false)
        {
            projectile.netUpdate = true;
            float npcDistance = float.PositiveInfinity;
            int npc = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                //this is bad but I don't know how to make it better.
                if (needsCanBeChasedBy)
                {
                    if (Main.npc[i].CanBeChasedBy())
                    {
                        if (Main.npc[i].active && Main.npc[i].damage > 0 && !Main.npc[i].friendly && projectile.Distance(Main.npc[i].Center) < npcDistance)
                        {
                            npcDistance = projectile.Distance(Main.npc[i].Center);
                            npc = i;
                        }
                    }
                }
                else
                {
                    if (Main.npc[i].active && Main.npc[i].damage > 0 && !Main.npc[i].friendly && projectile.Distance(Main.npc[i].Center) < npcDistance)
                    {
                        npcDistance = projectile.Distance(Main.npc[i].Center);
                        npc = i;
                    }
                }
            }
            return npc;
        }

        public static int TargetClosestPlayer(this Projectile projectile)
        {
            projectile.netUpdate = true;
            float playerDistance = float.PositiveInfinity;
            int player = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && projectile.Distance(Main.player[i].Center) < playerDistance)
                {
                    playerDistance = projectile.Distance(Main.player[i].Center);
                    player = i;
                }
            }
            return player;
        }

        public static void SetupMinion(this Projectile projectile, int minionSlots = 1)
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.minionSlots = minionSlots;
            projectile.penetrate = -1;
        }

        public const float gravity = 0.4f;
    }
}
