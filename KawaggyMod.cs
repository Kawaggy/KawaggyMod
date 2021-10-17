using KawaggyMod.Common.ModPlayers;
using KawaggyMod.Core;
using KawaggyMod.Core.Net;
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
                CustomizationManager.Load();
                Shaders.Load(this);
                ReadMe.GenerateOrUpdate(this);
                ChangeLog.GenerateOrUpdate(this);
            }
        }

        public override void PostSetupContent()
        {
            ModCompatibilityManager.Load();
        }

        public override void AddRecipeGroups()
        {
            RecipeManager.AddRecipeGroups(this);
        }

        public override void AddRecipes()
        {
            RecipeManager.AddRecipes(this);
        }

        public override void PostAddRecipes()
        {
            RecipeManager.EditRecipes(this);
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                CustomizationManager.Unload();
            }
            ModCompatibilityManager.Unload();
            PlayerEvents.Unload();
            SavePath = null;
            Instance = null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetHandler.HandlePacket(reader, whoAmI);
        }
    }
}