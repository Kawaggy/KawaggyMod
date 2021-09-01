using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod
{
	public class KawaggyMod : Mod
	{
		public static KawaggyMod Instance { get; private set; }
        public static string SavePath { get; private set; }

        public override void Load()
        {
            Instance = this;

            SavePath = Path.Combine(Main.SavePath, "Mod Specific Data", "KawaggyMod");

            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            if (!Main.dedServ)
            {
                ReadMe.GenerateOrUpdate(this);
                IceSwordsCustomization.Load();
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                IceSwordsCustomization.Unload();
            }
            SavePath = null;
            Instance = null;
        }
    }
}
