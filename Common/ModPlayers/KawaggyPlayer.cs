using KawaggyMod.Core.Net;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    // for the generic stuff that could be used for other things!
    public class KawaggyPlayer : ModPlayer
    {
        public bool oldJump;

        public override void Initialize()
        {
            oldJump = false;
        }

        public override void PostUpdateMiscEffects()
        {
            oldJump = player.releaseJump;
        }

        public override void clientClone(ModPlayer clientClone)
        {
            KawaggyPlayer clone = clientClone as KawaggyPlayer;

            clone.oldJump = oldJump;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            NetHandler.PlayerHandlers.playerHandler.SendOldJump(toWho, fromWho, player.whoAmI);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            KawaggyPlayer clone = clientPlayer as KawaggyPlayer;

            if (clone.oldJump != oldJump)
                NetHandler.PlayerHandlers.playerHandler.SendOldJump(-1, -1, player.whoAmI);
        }
    }
}
