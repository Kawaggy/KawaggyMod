using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KawaggyMod.Common.Worlds
{
    public class KawaggyWorld_Sadism : ModWorld
    {
        public static bool sadismMode;
        public static int bagFrame;
        public static int bagFrameCounter;

        public override void Initialize()
        {
            sadismMode = false;
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
            if (sadismMode) downed.Add("sadismMode");

            return new TagCompound
            {
                ["downed"] = downed
            };
        }

        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            sadismMode = downed.Contains("sadismMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = sadismMode;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            sadismMode = flags[0];
        }
    }
}
