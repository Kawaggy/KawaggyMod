using Terraria.ModLoader;

namespace KawaggyMod
{
	public class KawaggyMod : Mod
	{
		public static KawaggyMod Instance { get; private set; }

        public override void Load()
        {
            Instance = this;
        }
    }
}