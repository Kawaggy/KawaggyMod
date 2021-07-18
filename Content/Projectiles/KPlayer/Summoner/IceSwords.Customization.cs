using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public static class IceSwordsCustomization
    {
        public static int FrameAmount { get => TextureCache.Count + 5; }
        public static string SavePath { get; private set; }

        public static void Load()
        {
            SavePath = Path.Combine(KawaggyMod.SavePath, "IceSwords");

            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            TextureCache.cache = new List<(Texture2D, Rectangle)>();

            if (Directory.GetFiles(SavePath).Length != 0)
            {
                if (Main.IsGraphicsDeviceAvailable)
                {
                    string[] settings = Directory.GetFiles(SavePath, "*.txt");
                    string[] pictures = Directory.GetFiles(SavePath, "*.png");

                    foreach(string picture in pictures)
                    {
                        int frames = 1;
                        bool horizontal = true;
                        int padding = 0;

                        bool exists = false;
                        (bool isWrongFormat, string path, string reason) wrongFormat = (false, "", "");
                        foreach(string setting in settings)
                        {
                            if (Path.GetFileNameWithoutExtension(setting) == Path.GetFileNameWithoutExtension(picture))
                            {
                                exists = true;
                                string[] theSettings = File.ReadAllLines(setting);

                                if (theSettings.Length >= 3)
                                {
                                    if (theSettings[0].ToLower() != "horizontal" && theSettings[0].ToLower() == "vertical")
                                        horizontal = false;
                                    else
                                        wrongFormat = (true, Path.Combine(SavePath, Path.GetFileNameWithoutExtension(picture)) + ".txt", "direction | ");

                                    if (int.TryParse(theSettings[1], out int framesAmount))
                                        frames = (int)MathHelper.Clamp(framesAmount, 1, float.MaxValue);
                                    else
                                        wrongFormat = (true, Path.Combine(SavePath, Path.GetFileNameWithoutExtension(picture)) + ".txt", wrongFormat.reason + "frames | ");

                                    if (int.TryParse(theSettings[2], out int paddingAmount))
                                        padding = (int)MathHelper.Clamp(paddingAmount, 0, float.MaxValue);
                                    else
                                        wrongFormat = (true, Path.Combine(SavePath, Path.GetFileNameWithoutExtension(picture)) + ".txt", wrongFormat.reason + "padding");
                                }
                                else
                                    wrongFormat = (true, Path.Combine(SavePath, Path.GetFileNameWithoutExtension(picture)) + ".txt", "lines");
                            }
                        }

                        if (!exists)
                        {
                            string thePath = Path.Combine(SavePath, Path.GetFileNameWithoutExtension(picture)) + ".txt";
                            if (!File.Exists(thePath))
                            {
                                string[] lines = { "horizontal", "1", "0" };
                                File.WriteAllLines(thePath, lines);
                            }
                        }

                        if (wrongFormat.isWrongFormat)
                        {
                            KawaggyMod.Instance.Logger.Warn($"{wrongFormat.path} is the wrong format because of {wrongFormat.reason}! Using default settings instead");
                        }

                        using (var file = File.OpenRead(picture))
                        {
                            for (int i = 0; i < frames; i++)
                            {
                                Rectangle theRectangle;

                                if (horizontal)
                                    theRectangle = new Rectangle(i * (18 + padding), 0, 18, 44);
                                else
                                    theRectangle = new Rectangle(0, i * (44 + padding), 18, 44);

                                TextureCache.cache.Add((Texture2D.FromStream(Main.graphics.GraphicsDevice, file), theRectangle));
                            }
                        }
                    }

                    string word = TextureCache.Count > 1 ? "sprites" : "sprite";
                    KawaggyMod.Instance.Logger.Info($"Added {TextureCache.Count} {word} to cache for Ice Sword summon!");
                }
            }
        }

        public static void Unload()
        {
            TextureCache.cache.Clear();
            SavePath = null;
        }

        public static class TextureCache
        {
            public static List<(Texture2D texture, Rectangle frame)> cache;
            public static int Count
            {
                get => cache.Count;
            }
        }
    }
}
