using Terraria;

namespace KawaggyMod.Core.DataTypes
{
    public class NPCData
    {
        public NPC npc;
        public float distance;
        public bool hasLineOfSight;

        public NPCData(NPC npc, float distance, bool hasLineOfSight)
        {
            this.npc = npc;
            this.distance = distance;
            this.hasLineOfSight = hasLineOfSight;
        }
    }
}
