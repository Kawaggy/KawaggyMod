using KawaggyMod.Core.Net;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    // for the generic stuff that could be used for other things!
    public class KawaggyPlayer : ModPlayer
    {
        public bool oldJump;
        public float rotation;

        public override void Initialize()
        {
            oldJump = false;
            rotation = 0f;
        }

        public override void PostUpdateMiscEffects()
        {
            oldJump = player.releaseJump;
            rotation += MathHelper.TwoPi / 480;
            rotation = MathHelper.WrapAngle(rotation);
        }

        public override void clientClone(ModPlayer clientClone)
        {
            KawaggyPlayer clone = clientClone as KawaggyPlayer;

            clone.oldJump = oldJump;
            clone.rotation = rotation;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            NetHandler.PlayerHandlers.playerHandler.SendOldJumpAndRotation(toWho, fromWho, player.whoAmI);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            KawaggyPlayer clone = clientPlayer as KawaggyPlayer;

            if (clone.oldJump != oldJump)
            {
                NetHandler.PlayerHandlers.playerHandler.SendOldJumpAndRotation(-1, -1, player.whoAmI);
            }
        }
    }
}
