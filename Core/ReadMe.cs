using System.IO;
using Terraria.ModLoader;

namespace KawaggyMod.Core
{
    public class ReadMe
    {
        public static void GenerateOrUpdate(Mod mod)
        {
            string thePath = Path.Combine(KawaggyMod.SavePath, "READ_ME") + ".txt";

            string readMeVersion = "v0.0.0.4 Indev";
            string[] lines =
            {
                $"ReadMe - v{readMeVersion}",
                "Welcome! In here you will be able to customize many things in Kawaggy's Mod.",
                "Feel free to skip certain bits on here since you might be only interested in one thing only.",
                "Here will be guides on how to customize the bits for each thing in the mod.",
                "The current customizable things in Kawaggy's Mod are:",
                "- The Ice Swords (the sword itself)",
                "- The Cloud Summon (faces)",
                "",
                "---------------------------------------------------------------------------------------------------------",
                "",
                "0. Limitations",
                "There are size limitations to each customization, and these are...",
                "",
                "- Ice Swords:",
                "  Width = 18",
                "  Height = 44",
                "",
                "- Cloud Summon:",
                "  Width = 32",
                "  Height = 32",
                "",
                "---------------------------------------------------------------------------------------------------------",
                "",
                "1. The normal customization",
                "This applies to: Ice Swords, Cloud Summon",
                "",
                "In the respective folder you can place a .png in it and the mod automatically creates a .txt file for you!",
                "This .txt file is the same name as the .png and it serves as \"settings\" for the image.",
                "The layout is the following (in seperate lines):",
                "",
                "If it is horizontal or vertical (write 0 or 1, respectively)",
                "How many frames it has in the specified direction (the amount of frames)",
                "The padding between these frames (the amount of padding pixels)",
                "",
                "Here is an example, on which the frames are vertical, there are two frames, and there is no padding:",
                "horizontal=1",
                "frames=2",
                "padding=0",
                "",
                "---------------------------------------------------------------------------------------------------------"
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
                    mod.Logger.Info("...and is not up-to-date! Updating...");
                }
                else
                {
                    mod.Logger.Info("...and is up-to-date!");
                }
            }
        }
    }
}
