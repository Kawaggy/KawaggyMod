using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Content.Projectiles.KPlayer.Ranger
{
    public class BlueGelArrowProj : KProjectile
    {
        public override string Texture => Assets.Projectiles.Player + "Ranger/BlueGelArrowProj";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = 20.ToSeconds();
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1.ToSeconds();
        }

        public float State { get => projectile.localAI[0]; set => projectile.localAI[0] = value; }
        private const int Arrow = 0;
        private const int StickingOnEnemy = 1;
        private const int StickingOnTile = 2;

        public float StickTime { get => projectile.ai[0]; set => projectile.ai[0] = value; }
        public bool AllowPlatforms { get => projectile.ai[1] > 0; }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.localAI[0]);
            writer.Write(offset.X);
            writer.Write(offset.Y);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.localAI[0] = reader.ReadSingle();
            float X = reader.ReadSingle();
            float Y = reader.ReadSingle();
            offset = new Vector2(X, Y);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCKilled, projectile.position);
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100, new Color(58, 114, 255));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (State != StickingOnEnemy && State != StickingOnTile)
            {
                State = StickingOnTile;
                projectile.velocity.X *= 0;
            }
            return false;
        }

        Vector2 offset = Vector2.Zero;
        public int myNPC = -1; //NPC to stick to
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (myNPC == -1 && State == Arrow)
            {
                myNPC = target.whoAmI;
                offset = new Vector2(Main.rand.Next(-(target.height / 2), (target.height / 2) + 1),
                                     Main.rand.Next(-(target.width / 2), (target.width / 2) + 1));
                projectile.localAI[0] = StickingOnEnemy;
            }
        }

        public override void AI()
        {
            if (projectile.alpha > 50)
                projectile.alpha -= 15;
            if (projectile.alpha < 50)
                projectile.alpha = 50;

            switch (State)
            {
                case Arrow:
                    projectile.frame = 0;

                    projectile.velocity.Y += 0.2f;

                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

                    projectile.CapSpeed(16f);
                    break;

                case StickingOnEnemy: //dont care about speed here
                    if (myNPC != -1)
                    {
                        projectile.timeLeft = 30;
                        NPC npc = Main.npc[myNPC];

                        if (StickTime >= 0)
                            StickTime--;
                        if (StickTime == -1)
                            projectile.Kill();

                        if (!npc.active)
                            State = StickingOnTile;

                        projectile.rotation = npc.rotation;
                        projectile.position = npc.Center + offset.RotatedBy(npc.rotation);
                        projectile.frame = 1;
                    }
                    else
                    {
                        State = StickingOnTile;
                    }
                    break;

                case StickingOnTile:
                    projectile.timeLeft = 30;
                    projectile.frame = 1;

                    if (StickTime >= 0)
                        StickTime--;
                    if (StickTime == -1)
                        projectile.Kill();

                    int i = (int)((projectile.position.X + (projectile.width / 2)) / 16);
                    int j = (int)((projectile.position.Y + (projectile.height / 2)) / 16);

                    projectile.velocity.X *= 0f;

                    if (projectile.velocity.Y > 0.2f)
                        projectile.rotation += 0.1f;
                    else if (projectile.velocity.Y < 0)
                        projectile.rotation += 0.1f;

                    if (AllowPlatforms)
                    {
                        if (Main.tile[i, j].type == TileID.Platforms)
                        {
                            projectile.velocity *= 0;
                            return;
                        }
                    }

                    if (!WorldGen.SolidTile(i + projectile.direction, j))
                    {
                        projectile.velocity.Y += 0.2f;
                    }
                    projectile.CapSpeed(16f);
                    break;

                default: //if it somehow reaches this point it shouldnt bug out
                    KawaggyHelper.NewText("{$Mods.KawaggyMod.Common.BlueGelArrow}: State is not a valid state.");
                    projectile.active = false;
                    break;
            }
        }
    }
}
