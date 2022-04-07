using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KawaggyMod.Core.Helpers
{
    public static partial class TextureHelper
    {
        public static Color[] GetPixels1D(this Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            return colors1D;
        }

        public static Color GetPixel(this Color[] colors, int x, int y, int width)
        {
            return colors[x + (y * width)];
        }

        public static Color GetPixel(this Texture2D texture, int x, int y)
        {
            return texture.GetPixels1D().GetPixel(x, y, texture.Width);
        }

        public static void SetPixel(this ref Color color, Color set)
        {
            if (set == new Color(0, 0, 0, 0))
                return;

            color = set;
        }

        public static void ClearPixel(this ref Color color)
        {
            color = new Color(0, 0, 0, 0);
        }

        public static Color[,] GetPixels2D(this Color[] colors, int width, int height)
        {
            Color[,] colors2D = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors2D[x, y] = colors.GetPixel(x, y, width);
                }
            }
            return colors2D;
        }

        public static Color[,] GetPixels2D(this Texture2D texture)
        {
            return texture.GetPixels1D().GetPixels2D(texture.Width, texture.Height);
        }

        public static Color GetAverageColor(this Color[] colors)
        {
            float r = 0;
            float g = 0;
            float b = 0;
            int count = 0;
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] != new Color(0, 0, 0, 0))
                {
                    count++;
                    r += colors[i].R;
                    g += colors[i].G;
                    b += colors[i].B;
                }
            }

            r /= count;
            g /= count;
            b /= count;

            return new Color((byte)r, (byte)g, (byte)b, 255);
        }

        public static Color GetAverageColor(this Texture2D texture)
        {
            return texture.GetPixels1D().GetAverageColor();
        }

        public static Color HexToARGB(this string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
            byte r = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
            byte g = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
            byte b = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
            return new Color(r, g, b, a);
        }

        public static Color HexToRGB(this string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
            byte g = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
            byte b = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);
            return new Color(r, g, b);
        }

        public static string RGBToHex(this Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static string ARGBToHex(this Color color)
        {
            return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static class VanillaColors
        {
            public static Color Chat
            {
                get => HexToRGB("#FFFFFF");
            }

            public static Color Event
            {
                get => HexToRGB("#32FF82");
            }

            public static Color Invasion
            {
                get => HexToRGB("#AF4BFF");
            }

            public static Color BossSpawn
            {
                get => HexToRGB("#AF4BFF");
            }

            public static Color PlayerSlain
            {
                get => HexToRGB("#E11919");
            }

            public static Color NPCSlain
            {
                get => HexToRGB("#FF1919");
            }

            public static Color NPCArrived
            {
                get => HexToRGB("#327DFF");
            }

            public static Color Status
            {
                get => HexToRGB("#FFF014");
            }

            public static Color Party
            {
                get => HexToRGB("#FF00A0");
            }
        }
    }
}
