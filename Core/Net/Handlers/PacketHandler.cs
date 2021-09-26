using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Net.Handlers
{
    public abstract class PacketHandler
    {
        internal HandlerType Type { get; private set; }

        protected PacketHandler(HandlerType handlerType)
        {
            Type = handlerType;
        }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected ModPacket GetPacket(byte messageType, int fromWho)
        {
            var packet = KawaggyMod.Instance.GetPacket();
            packet.Write((byte)Type);
            packet.Write(messageType);
            /*
            if (Main.netMode == NetmodeID.Server)
            {
                packet.Write((byte)fromWho);
            }
            */
            return packet;
        }

        protected void NoMessageTypeFound<T>(byte messageType) where T : PacketHandler
        {
            KawaggyMod.Instance.Logger.WarnFormat("KawaggyMod: Unkown message type for {0}: {1}", new object[] { nameof(T), messageType });
        }
    }
}
