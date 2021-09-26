using KawaggyMod.Core.Net;
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
    }
}
