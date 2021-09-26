using KawaggyMod.Common.ModPlayers;
using KawaggyMod.Core.Helpers;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Net.Handlers
{
    public class KawaggyPlayerHandler : PacketHandler
    {
        public enum MessageType : byte
        {
            SyncOldJump,
        }

        public KawaggyPlayerHandler() : base(HandlerType.KawaggyPlayerHandler) { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            MessageType message = (MessageType)reader.ReadByte();

            switch(message)
            {
                case MessageType.SyncOldJump:
                    ReceiveOldJump(reader);
                    break;

                default:
                    NoMessageTypeFound<KawaggyPlayerHandler>((byte)message);
                    break;
            }
        }

        public void SendOldJump(int toWho, int fromWho, int player)
        {
            ModPacket packet = GetPacket((byte)MessageType.SyncOldJump, fromWho);
            packet.Write((byte)player);
            KawaggyPlayer kawaggyPlayer = Main.player[player].Kawaggy();
            packet.Write(kawaggyPlayer.oldJump);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveOldJump(BinaryReader reader)
        {
            byte player = reader.ReadByte();
            KawaggyPlayer kawaggyPlayer = Main.player[player].Kawaggy();
            kawaggyPlayer.oldJump = reader.ReadBoolean();

            if (Main.netMode == NetmodeID.Server)
            {
                SendOldJump(-1, player, player);
            }
        }
    }
}
