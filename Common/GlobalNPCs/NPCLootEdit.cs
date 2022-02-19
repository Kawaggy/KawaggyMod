using KawaggyMod.Content.Items.Miscellaneous.Materials;
using KawaggyMod.Core.Helpers;
using KawaggyMod.Core.Structs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Common.GlobalNPCs
{
    public class NPCLootEdit : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            EditNonExpertDrops(npc);
        }

        private static void EditNonExpertDrops(NPC npc)
        {
            if (!Main.expertMode)
            {
                switch (npc.type)
                {
                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:

                        int count = 0;

                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC otherNpc = Main.npc[i];
                            if (i != npc.whoAmI)
                            {
                                if (otherNpc.active)
                                {
                                    if (otherNpc.type == NPCID.EaterofWorldsBody || otherNpc.type == NPCID.EaterofWorldsHead || otherNpc.type == NPCID.EaterofWorldsTail)
                                    {
                                        count++;
                                    }
                                }
                            }
                        }

                        if (count == 0)
                            npc.DropItem(new ItemDropInfo(type: ModContent.ItemType<EaterOfWorldsTooth>(), dropPerPlayer: false, min: 8, max: 21));

                        break;

                    case NPCID.BrainofCthulhu:

                        npc.DropItem(new ItemDropInfo(type: ModContent.ItemType<BrainOfCthulhuTooth>(), dropPerPlayer: false, min: 8, max: 21));

                        break;
                }
            }
        }
    }
}
