using KawaggyMod.Common.ModPlayers;
using KawaggyMod.Core.Helpers;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Net.Handlers.PlayerHandlers
{
    public class KawaggyPlayerHandler : PacketHandler
    {
        public enum MessageType : byte
        {
            SyncOldJumpAndRotation,
        }

        public KawaggyPlayerHandler() : base(HandlerType.KawaggyPlayerHandler) { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            MessageType message = (MessageType)reader.ReadByte();

            switch(message)
            {
                case MessageType.SyncOldJumpAndRotation:
                    ReceiveOldJumpAndRotation(reader);
                    break;

                default:
                    NoMessageTypeFound<KawaggyPlayerHandler>((byte)message);
                    break;
            }
        }

        public void SendOldJumpAndRotation(int toWho, int fromWho, int player)
        {
            ModPacket packet = GetPacket((byte)MessageType.SyncOldJumpAndRotation);

            packet.Write((byte)player);

            KawaggyPlayer kawaggyPlayer = Main.player[player].Kawaggy();
            packet.Write(kawaggyPlayer.oldJump);
            packet.Write(kawaggyPlayer.rotation);

            packet.Send(toWho, fromWho);
        }

        public void ReceiveOldJumpAndRotation(BinaryReader reader)
        {
            byte player = reader.ReadByte();

            KawaggyPlayer kawaggyPlayer = Main.player[player].Kawaggy();
            kawaggyPlayer.oldJump = reader.ReadBoolean();
            kawaggyPlayer.rotation = reader.ReadSingle();

            if (Main.netMode == NetmodeID.Server)
            {
                SendOldJumpAndRotation(-1, player, player);
            }
        }
    }
}
