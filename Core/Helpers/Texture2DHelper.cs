using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KawaggyMod.Core.Helpers
{
    public static class Texture2DHelper
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

        public static Color GetColorAverage(this Color[] colors)
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

        public static Color GetColorAverage(this Texture2D texture)
        {
            return texture.GetPixels1D().GetColorAverage();
        }

        public static string ToHexARGB(this Color color, bool includeHash = false)
        {
            string[] argb =
            {
                color.A.ToString("X2"),
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2")
            };
            return (includeHash ? "#" : string.Empty) + string.Join(string.Empty, argb);
        }

        public static string ToHexRGB(this Color color, bool includeHash = false)
        {
            string[] rgb =
            {
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2")
            };
            return (includeHash ? "#" : string.Empty) + string.Join(string.Empty, rgb);
        }

        public static string ToARGB(this Color color, bool includeComma = false)
        {
            string[] argb =
            {
                color.A.ToString(),
                color.R.ToString(),
                color.G.ToString(),
                color.B.ToString()
            };
            return string.Join(includeComma ? ", " : string.Empty, argb);
        }

        public static string ToRGB(this Color color, bool includeComma = false)
        {
            string[] rgb =
            {
                color.R.ToString(),
                color.G.ToString(),
                color.B.ToString()
            };
            return string.Join(includeComma ? ", " : string.Empty, rgb);
        }
    }
}
