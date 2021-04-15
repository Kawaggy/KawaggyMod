using Microsoft.Xna.Framework;

namespace KawaggyMod.Core.Systems.Verlet
{
    public struct VerletSegment
    {
        public Vector2 position;
        public Vector2 oldPosition;
        public Vector2 center;

        public VerletSegment(Vector2 position)
        {
            this.position = position;
            this.oldPosition = position;
            this.center = position;
        }
    }
}
