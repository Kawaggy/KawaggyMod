using KawaggyMod.Core;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class MiniBlueSlime : KProjectile
    {
        public override string Texture => Assets.Projectiles.Player + "Summoner/MiniBlueSlime";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 12;
            projectile.aiStyle = 26;
            aiType = 266;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCKilled, projectile.position);
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, Main.rand.Next(-5, 5), -2, 100, new Color(58, 114, 255));
            }
        }

        public int TimeLeft { get => (int)projectile.ai[0]; }

        private bool FirstTick = false;
        private int timer = 0;
        public override bool PreAI()
        {
            if (!FirstTick)
            {
                timer = TimeLeft;
                FirstTick = true;
            }

            Player player = Main.player[projectile.owner];

            if (player.dead || !player.active)
            {
                if (timer > 30)
                    timer = 30;
            }

            timer--;
            return true;
        }

        public override void PostAI()
        {
            if (timer <= 0)
                projectile.Kill();
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(timer);
        public override void ReceiveExtraAI(BinaryReader reader) => timer = reader.ReadInt32();
    }
}
