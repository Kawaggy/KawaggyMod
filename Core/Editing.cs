using KawaggyMod.Common.Events;
using KawaggyMod.Common.ModWorlds;
using Terraria;
using Terraria.ID;

namespace KawaggyMod.Core
{
    public static class Editing
    {
        public delegate void DayEventDelegate();
        /// <summary>
        /// Allows you to add an event exactly when a new day starts
        /// </summary>
        public static event DayEventDelegate DayEvent;

        public static void Unload()
        {
            DayEvent = null;
        }

        public static void OnEdits()
        {
            On.Terraria.GameContent.Events.Sandstorm.StopSandstorm += CheckIfDownedSandstorm;
            On.Terraria.Main.stopMoonEvent += AddDayEvents;
        }

        public static void ILEdits()
        {

        }

        private static void AddDayEvents(On.Terraria.Main.orig_stopMoonEvent orig)
        {
            orig();
            if (Main.time == 0.0 && !Main.dayTime)
            {
                DayEvent?.Invoke();
            }
        }

        private static void CheckIfDownedSandstorm(On.Terraria.GameContent.Events.Sandstorm.orig_StopSandstorm orig)
        {
            if (Terraria.GameContent.Events.Sandstorm.TimeLeft <= 0)
            {
                KawaggyWorld.downedSandstorm = true;
            }

            orig();
        }
    }
}
