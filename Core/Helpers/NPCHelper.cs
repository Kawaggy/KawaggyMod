using Microsoft.Xna.Framework;
using Terraria;

namespace KawaggyMod.Core.Helpers
{
    public static class NPCHelper
    {
        public static void NewNPC(Vector2 position, int Type, int Start = 0, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int Target = 255)
        {
            NPC.NewNPC((int)position.X, (int)position.Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        }

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
