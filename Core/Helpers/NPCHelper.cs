using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static class NPCHelper
    {
        public static void AllBuffImmune(this NPC npc)
        {
            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }
        }

        public static void Butcher(this NPC npc)
        {
            npc.life = 0;
            npc.HitEffect(0, npc.lifeMax);
            npc.checkDead();
            npc.NPCLoot();
            npc.active = false;
            npc.netUpdate = true;
        }
    }
}
