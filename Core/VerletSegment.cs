using Microsoft.Xna.Framework;

namespace KawaggyMod.Core
{
    public struct VerletSegment
    {
        public Vector2 position;
        public Vector2 oldPosition;
        public Vector2 center;

        public VerletSegment(Vector2 pos)
        {
            position = pos;
            oldPosition = pos;
            center = pos;
        }
    }
}
