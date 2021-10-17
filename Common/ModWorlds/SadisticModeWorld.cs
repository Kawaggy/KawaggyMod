using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KawaggyMod.Common.ModWorlds
{
    public class SadisticModeWorld : ModWorld
    {
        public static bool sadisticMode;

        public override void Initialize()
        {
            sadisticMode = false;
        }

        public override void PostUpdate()
        {
            if (!Main.expertMode)
                sadisticMode = false;
        }

        public override TagCompound Save()
        {
            List<string> downed = new List<string>();

            if (sadisticMode)
                downed.Add("sadisticMode");

            return new TagCompound
            {
                { "downed", downed }
            };
        }

        public override void Load(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("downed");

            sadisticMode = downed.Contains("sadisticMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte
            {
                [0] = sadisticMode
            };

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            sadisticMode = flags[0];
        }
    }
}
