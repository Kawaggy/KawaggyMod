using KawaggyMod.Common.ModPlayers;
using KawaggyMod.Core.Helpers;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Net.Handlers
{
    public class SummonPlayerHandler : PacketHandler
    {
        public enum MessageType : byte
        {
            SyncCloud,
        }

        public SummonPlayerHandler() : base(HandlerType.SummonPlayerHandler) { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            MessageType message = (MessageType)reader.ReadByte();

            switch(message)
            {
                case MessageType.SyncCloud:
                    ReceiveCloud(reader);
                    break;

                default:
                    NoMessageTypeFound<SummonPlayerHandler>((byte)message);
                    break;
            }
        }

        public void SendCloud(int toWho, int fromWho, int player)
        {
            ModPacket packet = GetPacket((byte)MessageType.SyncCloud, fromWho);
            packet.Write((byte)player);
            SummonPlayer summonPlayer = Main.player[player].Summons();
            packet.Write(summonPlayer.jumpAgainCloudCounter);
            packet.Write(summonPlayer.currentCloudJump);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveCloud(BinaryReader reader)
        {
            byte player = reader.ReadByte();
            Main.player[player].Summons().jumpAgainCloudCounter = reader.ReadInt32();
            Main.player[player].Summons().currentCloudJump = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendCloud(-1, player, player);
                return;
            }
        }
    }
}
