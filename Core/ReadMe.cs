using System.IO;
using Terraria.ModLoader;

namespace KawaggyMod.Core
{
    public class ReadMe
    {
        public static void GenerateOrUpdate(Mod mod)
        {
            string thePath = Path.Combine(KawaggyMod.SavePath, "READ_ME") + ".txt";

            string readMeVersion = "v0.0.0.3 Indev";
            string[] lines =
            {
                $"ReadMe - v{readMeVersion}",
                "Welcome! In here you will be able to customize many things in Kawaggy's Mod.",
                "Feel free to skip certain bits on here since you might be only interested in one thing only.",
                "Here will be guides on how to customize the bits for each thing in the mod.",
                "",
                "---------------------------------------------------------------------------------------------------------",
                "1. Ice Swords",
                "",
                "In the IceSwords folder you can place a .png in it and the mod automatically creates a .txt file for you!",
                "This .txt file is the same name as the .png and it serves as \"settings\" for the image.",
                "The layout is the following (in seperate lines):",
                "",
                "If it is vertical or horizontal (write \"vertical\" or \"horizontal\" without the quotation marks)",
                "How many frames it has in the specified direction (a number)",
                "The padding between these frames (a number, pixels)"
            };

            if (!File.Exists(thePath))
            {
                mod.Logger.Info("READ_ME did not exist!");
                File.WriteAllLines(thePath, lines);
            }
            else
            {
                mod.Logger.Info("READ_ME did exist...");
                if (File.ReadAllLines(thePath)[0] != $"ReadMe - v{readMeVersion}")
                {
                    File.WriteAllLines(thePath, lines);
                    mod.Logger.Info("...and is not up-to-date!");
                }
                else
                {
                    mod.Logger.Info("...and is up-to-date!");
                }
            }
        }
    }
}
