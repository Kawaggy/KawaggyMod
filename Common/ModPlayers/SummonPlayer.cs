using KawaggyMod.Core.Net;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    public class SummonPlayer : ModPlayer
    {
        public int jumpAgainCloudCounter;
        public int currentCloudJump;
        
        public override void Initialize()
        {
            jumpAgainCloudCounter = 0;
            currentCloudJump = -1;
        }

        public override void ResetEffects()
        {
            if (jumpAgainCloudCounter > 0)
                jumpAgainCloudCounter--;

            if (player.velocity.Y == 0)
            {
                currentCloudJump = -1;
            }
        }

        public override void clientClone(ModPlayer clientClone)
        {
            SummonPlayer clone = clientClone as SummonPlayer;

            clone.jumpAgainCloudCounter = jumpAgainCloudCounter;
            clone.currentCloudJump = currentCloudJump;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            NetHandler.PlayerHandlers.summonHandler.SendCloud(toWho, fromWho, player.whoAmI);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            SummonPlayer clone = clientPlayer as SummonPlayer;

            if (clone.currentCloudJump != currentCloudJump)
            {
                NetHandler.PlayerHandlers.summonHandler.SendCloud(-1, -1, player.whoAmI);
            }
        }

        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
        {
            bool playerHasSummonOut = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == player.whoAmI)
                {
                    if (projectile.minion && projectile.minionSlots > 0)
                    {
                        playerHasSummonOut = true;
                        break;
                    }
                }
            }

            if (playerHasSummonOut)
            {
                if (!item.summon && !item.sentry)
                {
                    if (!Main.hardMode)
                    {
                        mult *= 0.85f;
                        return;
                    }

                    if (NPC.downedMoonlord)
                    {
                        mult *= 0.25f;
                        return;
                    }

                    if (Main.hardMode)
                    {
                        mult *= 0.5f;
                        return;
                    }
                }
            }
        }
    }
}
