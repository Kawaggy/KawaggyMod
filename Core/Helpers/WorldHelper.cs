using KawaggyMod.Common.ModWorlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core.Helpers
{
    public static class WorldHelper
    {
        public static class Tiles
        {
            public static int PureSandBlocks
            {
                get
                {
                    return 
                        Main.screenTileCounts[TileID.Sand] + 
                        Main.screenTileCounts[TileID.Sandstone] + 
                        Main.screenTileCounts[TileID.HardenedSand];
                }
            }

            public static int CorruptSandBlocks
            {
                get
                {
                    return
                        Main.screenTileCounts[TileID.Ebonsand] + 
                        Main.screenTileCounts[TileID.CorruptSandstone] + 
                        Main.screenTileCounts[TileID.CorruptHardenedSand];
                }
            }

            public static int CrimsonSandBlocks
            {
                get
                {
                    return
                        Main.screenTileCounts[TileID.Crimsand] + 
                        Main.screenTileCounts[TileID.CrimsonSandstone] + 
                        Main.screenTileCounts[TileID.CrimsonHardenedSand];
                }
            }

            public static int HallowSandBlocks
            {
                get
                {
                    return 
                        Main.screenTileCounts[TileID.Pearlsand] + 
                        Main.screenTileCounts[TileID.HallowSandstone] + 
                        Main.screenTileCounts[TileID.HallowHardenedSand];
                }
            }
        }

        public static void StopSandstorm()
        {
            ReflectionCache.stopSandstorm?.Invoke(null, null);
        }

        public static void SendWorldData()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.WorldData);
        }

        public static bool CanMassiveSandstormStart
        {
            get
            {
                return KawaggyWorld.downedSandstorm && NPC.downedBoss3;
            }
        }
    }
}
