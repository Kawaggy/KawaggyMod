using KawaggyMod.Core.Net.Handlers;
using System.IO;

namespace KawaggyMod.Core.Net
{
    public class NetHandler
    {
        public class PlayerHandlers
        {
            internal static SummonPlayerHandler summonHandler = new SummonPlayerHandler();
            internal static KawaggyPlayerHandler playerHandler = new KawaggyPlayerHandler();
        }

        public static void HandlePacket(BinaryReader reader, int fromWho)
        {
            HandlerType handlerType = (HandlerType)reader.ReadByte();

            switch(handlerType)
            {
                case HandlerType.KawaggyPlayerHandler:
                    PlayerHandlers.playerHandler.HandlePacket(reader, fromWho);
                    break;

                case HandlerType.SummonPlayerHandler:
                    PlayerHandlers.summonHandler.HandlePacket(reader, fromWho);
                    break;
            }
        }
    }
}
