using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KawaggyMod.Core
{
    public class FramedTexture2D
    {
        public Texture2D texture;
        public Rectangle frame;

        public FramedTexture2D(Texture2D texture, Rectangle frame)
        {
            this.texture = texture;
            this.frame = frame;
        }
    }
}
