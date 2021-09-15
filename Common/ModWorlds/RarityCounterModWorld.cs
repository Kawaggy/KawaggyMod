using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModWorlds
{
    public class RarityCounterModWorld : ModWorld
    {
        public static float fourSecondColorLerp;
        internal float fourSecondColorLerpCounter;
        public override void Initialize()
        {
            fourSecondColorLerp = 0f;
            fourSecondColorLerpCounter = 0f;
        }

        public override void PostUpdate()
        {
            fourSecondColorLerp = CountUp(ref fourSecondColorLerpCounter, 240f);
        }

        internal float CountUp(ref float toLerp, float counter)
        {
            toLerp += MathHelper.TwoPi / counter;
            toLerp = MathHelper.WrapAngle(toLerp);
            return (float)(Math.Sin(toLerp) / 2) + 0.5f;
        }
    }
}
