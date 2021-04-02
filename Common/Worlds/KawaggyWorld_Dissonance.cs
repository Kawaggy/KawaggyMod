using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KawaggyMod.Common.Worlds
{
    public class KawaggyWorld_Dissonance : ModWorld
    {
        public static bool dissonanceMode;
        public static int bagFrame;
        public static int bagFrameCounter;

        public override void Initialize()
        {
            dissonanceMode = false;
            bagFrame = 0;
            bagFrameCounter = 0;
        }

        public override void PostUpdate()
        {
            if (bagFrameCounter++ > 5)
            {
                bagFrameCounter = 0;
                bagFrame++;
                if (bagFrame >= 4)
                    bagFrame = 0;
            }
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (dissonanceMode) downed.Add("dissonanceMode");

            return new TagCompound
            {
                ["downed"] = downed
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            dissonanceMode = downed.Contains("dissonanceMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = dissonanceMode;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            dissonanceMode = flags[0];
        }
    }
}
