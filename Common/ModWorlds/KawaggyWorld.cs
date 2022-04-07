using KawaggyMod.Common.Events;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KawaggyMod.Common.ModWorlds
{
    public class KawaggyWorld : ModWorld
    {
        public static bool downedSandstorm;
        public static bool downedMassiveSandstorm;

        public override void Initialize()
        {
            downedSandstorm = false;
            downedMassiveSandstorm = false;
        }

        public override void PostUpdate()
        {
            MassiveSandstorm.UpdateTime();
        }

        public override TagCompound Save()
        {
            List<string> downed = new List<string>();

            if (downedSandstorm)
                downed.Add("downedSandstorm");

            if (downedMassiveSandstorm)
                downed.Add("downedMassiveSandstorm");

            return new TagCompound()
            {
                { "downed", downed }
            };
        }

        public override void Load(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("downed");

            if (downed.Contains("downedSandstorm"))
                downedSandstorm = true;

            if (downed.Contains("downedMassiveSandstorm"))
                downedMassiveSandstorm = true;
        }

        public override void NetSend(BinaryWriter writer)
        {
            MassiveSandstorm.SendData(writer);

            BitsByte flags = new BitsByte
            {
                [0] = downedSandstorm,
                [1] = downedMassiveSandstorm
            };

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            MassiveSandstorm.ReceiveData(reader);

            BitsByte flags = reader.ReadByte();

            downedSandstorm = flags[0];
            downedMassiveSandstorm = flags[1];
        }
    }
}
