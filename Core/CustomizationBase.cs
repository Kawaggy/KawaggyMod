using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;

namespace KawaggyMod.Core
{
    public abstract class CustomizationBase
    {
        public CustomizationBase(string customizationObject, int width, int height)
        {
            this.customizationObject = customizationObject;
            this.width = width;
            this.height = height;
        }

        public string customizationObject;
        public int width;
        public int height;

        public string SavePath
        {
            get
            {
                return Path.Combine(KawaggyMod.SavePath, customizationObject);
            }
        }

        public abstract int FrameCount { get; set; }

        public List<(Texture2D texture, Rectangle frame)> cache;

        /// <summary>
        /// For custom loading of images. Has SimpleLoad called inside it.
        /// </summary>
        public virtual void Load()
        {
            SimpleLoad();
        }

        /// <summary>
        /// Default load system.
        /// </summary>
        public void SimpleLoad()
        {
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            cache = new List<(Texture2D texture, Rectangle frame)>();

            if (Directory.GetFiles(SavePath).Length != 0)
            {
                if (Main.IsGraphicsDeviceAvailable)
                {
                    string[] settings = Directory.GetFiles(SavePath, "*.txt");
                    string[] images = Directory.GetFiles(SavePath, "*.png");

                    foreach (string image in images)
                    {
                        int frames = 1;
                        int horizontal = 0;
                        int padding = 0;

                        bool settingsExist = false;
                        (bool isWrongFormat, string path, string reason) wrongFormat = (false, "", "");

                        foreach (string setting in settings)
                        {
                            if (Path.GetFileNameWithoutExtension(setting) == Path.GetFileNameWithoutExtension(image))
                            {
                                settingsExist = true;

                                string[] theSettings = File.ReadAllLines(setting);

                                string thePath = Path.GetFileNameWithoutExtension(image);

                                char[] theChar = "=".ToCharArray();
                                if (theSettings.Length >= 3)
                                {
                                    try
                                    {
                                        string[] horizontalLine = theSettings[0].Split(theChar);
                                        if (horizontalLine.Length >= 2)
                                        {
                                            if (int.TryParse(horizontalLine[1], out int horizontalSetting))
                                                horizontal = horizontalSetting;
                                            else
                                                wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "direction, ");
                                        }
                                        else
                                            wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "direction line, ");

                                        string[] framesLine = theSettings[1].Split(theChar);
                                        if (framesLine.Length >= 2)
                                        {
                                            if (int.TryParse(framesLine[1], out int framesCount))
                                                frames = (int)MathHelper.Clamp(framesCount, 1, float.MaxValue);
                                            else
                                                wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "frames, ");
                                        }
                                        else
                                            wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "frames line, ");

                                        string[] paddingLine = theSettings[2].Split(theChar);
                                        if (paddingLine.Length >= 2)
                                        {
                                            if (int.TryParse(paddingLine[1], out int paddingAmount))
                                                padding = (int)MathHelper.Clamp(paddingAmount, 0, float.MaxValue);
                                            else
                                                wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "padding");
                                        }
                                        else
                                            wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "padding line");
                                    }
                                    catch (Exception e)
                                    {
                                        KawaggyMod.Instance.Logger.Error(e);
                                    }
                                }
                                else
                                    wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "line count");
                            }
                        }

                        if (!settingsExist)
                        {
                            string thePath = Path.Combine(SavePath, Path.GetFileNameWithoutExtension(image)) + ".txt";
                            if (!File.Exists(thePath))
                            {
                                string[] lines = { "horizontal=0", "frames=1", "padding=0" };
                                File.WriteAllLines(thePath, lines);
                            }
                            KawaggyMod.Instance.Logger.Info($"Created settings file for {Path.Combine(SavePath, Path.GetFileNameWithoutExtension(image))}.png");
                        }

                        if (wrongFormat.isWrongFormat)
                        {
                            KawaggyMod.Instance.Logger.Warn($"{wrongFormat.path} is the wrong format because of {wrongFormat.reason}! Using default settings instead.");
                        }

                        using (FileStream file = File.OpenRead(image))
                        {
                            for (int i = 0; i < frames; i++)
                            {
                                Rectangle frame;
                                switch (horizontal)
                                {
                                    case 0:
                                        frame = new Rectangle(i * (width + padding), 0, width, height);
                                        break;

                                    case 1:
                                        frame = new Rectangle(0, i * (height + padding), width, height);
                                        break;

                                    case 2:
                                        KawaggyMod.Instance.Logger.Info("horizontal setting '2' is not implemented yet!");
                                        frame = Rectangle.Empty;
                                        break;

                                    default:
                                        frame = Rectangle.Empty;
                                        break;
                                }

                                if (frame != Rectangle.Empty)
                                {
                                    cache.Add((Texture2D.FromStream(Main.graphics.GraphicsDevice, file), frame));
                                }
                            }
                        }
                    }

                    string word = cache.Count > 1 ? "sprites" : "sprite";
                    KawaggyMod.Instance.Logger.Info($"Added {cache.Count} {word} to cache for {customizationObject}!");
                }
            }
        }

        public virtual void AdditionalUnloading()
        {
        }

        public void Unload()
        {
            cache.Clear();
            cache = null;
            FrameCount = 0;
            customizationObject = null;
            width = 0;
            height = 0;
            AdditionalUnloading();
        }
    }
}