using KawaggyMod.Core.Net.Handlers;
using KawaggyMod.Core.Net.Handlers.PlayerHandlers;
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
            HandlerType handlerType = (HandlerType)reader.ReadUInt32();

            switch(handlerType)
            {
                case HandlerType.KawaggyPlayerHandler:
                    PlayerHandlers.playerHandler.HandlePacket(reader, fromWho);
                    break;

                case HandlerType.SummonPlayerHandler:
                    PlayerHandlers.summonHandler.HandlePacket(reader, fromWho);
                    break;

                default:
                    KawaggyMod.Instance.Logger.WarnFormat("No handler found of id {0}!", (uint)handlerType);
                    break;
            }
        }
    }
}
