using System.Reflection;
using Terraria.GameContent.Events;

namespace KawaggyMod.Core
{
    public static class ReflectionCache
    {
        public static MethodInfo stopSandstorm;

        public static void Load()
        {
            stopSandstorm = typeof(Sandstorm).GetMethod("StopSandstorm", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void PostSetupContentLoad()
        {

        }

        public static void Unload()
        {
            stopSandstorm = null;
        }
    }
}
