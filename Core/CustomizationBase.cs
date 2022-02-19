using KawaggyMod.Core.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;

namespace KawaggyMod.Core
{
    //This is better done in a different mod?

    //TODO: REWORK
    // Add a list of textures that get loaded only once, and for every "frame" have a rectangle and
    //a string/int for an ID for the given texture. then have a method that returns a
    //FramedTexture2D with the given texture and the given frame.

    //public List<Texture2D> textureCache;
    //public Dictionary<int, int> textureInfo;
    //public FramedTexture2D GetTexture(int frame)

    //since this system works in frames, each texture could have an int for amount of frames. so maybe a dictionary is easier?
    //public Dictionary<Texture2D, int> cache;
    //then just a public FramedTexture2D GetTexture(int frame) where the frame is used for the int part of the Dictionary

    //with the settings, one could read the amount of frames for each Texture and have vertical and horizontal.
    //then, in the dictionary, have the amount of frames inside of the int.
    //then when one wants a specific frame, one could just count all the frames (maybe a new static int called Frames) to then
    //use it. then use the int part of the dictionary and have it as the max. (min 1, max int.MaxValue).
    //it could also now finally use the two rows, so that horizontal is used first and then goes down for each row,
    //though that might need a new setting for each frame (horizontalFrames and verticalFrames), would get rid of frames and horizontal
    //settings.

    //this is simply to reduce the amount of ram used by the mod by loading the same texture n times for each frame that uses
    //the same texture
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

        public abstract int FrameCount { get; }

        public List<FramedTexture2D> cache;

        /// <summary>
        /// For custom loading of images. Has SimpleLoad called inside the base.
        /// </summary>
        public virtual void Load()
        {
            SimpleLoad();
        }


        //if above rework is done, have to rework the settings and loading as well here
        /// <summary>
        /// Default load system.
        /// </summary>
        public void SimpleLoad()
        {
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            cache = new List<FramedTexture2D>();

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
                                if (theSettings.Length >= 1)
                                {
                                    try
                                    {
                                        for (int i = 0; i < theSettings.Length; i++)
                                        {
                                            ReadSetting(theSettings[i].Split(theChar), thePath, ref horizontal, ref frames, ref padding, ref wrongFormat);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        KawaggyMod.Instance.Logger.Error(e);
                                    }
                                }
                                else
                                    wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "no lines found");
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
                                    cache.Add(new FramedTexture2D(Texture2D.FromStream(Main.graphics.GraphicsDevice, file), frame));
                                }
                            }
                        }
                    }

                    string word = cache.Count > 1 ? "sprites" : "sprite";
                    KawaggyMod.Instance.Logger.Info($"Added {cache.Count} {word} to cache for {customizationObject}!");
                }
            }
            else
            {
                KawaggyMod.Instance.Logger.Info($"No sprites found for {customizationObject}.");
            }
        }

        internal void ReadSetting(string[] line, string thePath, ref int horizontal, ref int frames, ref int padding, ref (bool isWrongFormat, string path, string reason) wrongFormat)
        {
            if (line.Length >= 2)
            {
                switch(line[0].ToLower())
                {
                    case "horizontal":
                        if (int.TryParse(line[1], out int horizontalSetting))
                            horizontal = horizontalSetting;
                        else
                            wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "direction, ");
                        break;

                    case "frames":
                        if (int.TryParse(line[1], out int framesCount))
                            frames = (int)MathHelper.Clamp(framesCount, 1, float.MaxValue);
                        else
                            wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "frames, ");
                        break;

                    case "padding":
                        if (int.TryParse(line[1], out int paddingAmount))
                            padding = (int)MathHelper.Clamp(paddingAmount, 0, float.MaxValue);
                        else
                            wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "padding");
                        break;

                    default:
                        wrongFormat = (true, Path.Combine(SavePath, thePath) + ".txt", wrongFormat.reason + "unkown line setting");
                        break;
                }
            }
        }

        public virtual void AdditionalUnloading()
        {
        }

        /// <summary>
        /// Already unloads the cache, customization object, data for width and height. At the end, calls AdditionalUnloading
        /// </summary>
        public void Unload()
        {
            cache.Clear();
            cache = null;
            customizationObject = null;
            width = 0;
            height = 0;
            AdditionalUnloading();
        }
    }
}