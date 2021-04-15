using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;

namespace KawaggyMod.Core.Systems.Verlet
{
    public class Verlet
    {
        public List<VerletSegment> segments;
        public bool FirstTick;
        public float height;
        public int segmentNum;
        public int width;
        public int constraintSimulations;

        public Verlet()
        {
            segments = new List<VerletSegment>();
            segmentNum = 32;
            height = 8f;
            width = 6;
            constraintSimulations = 50;
            FirstTick = true;
        }

        public Verlet(int segmentAmount, float height, int width, int constraintSimulations)
        {
            segments = new List<VerletSegment>();
            segmentNum = segmentAmount;
            this.height = height;
            this.width = width;
            this.constraintSimulations = constraintSimulations;
            FirstTick = true;
        }

        public virtual void StartAttachTo(ref VerletSegment start)
        {
        }

        public virtual void EndAttachTo(ref VerletSegment end)
        {
        }

        public void Dispose()
        {
            OnDispose();
            segments = null;
            FirstTick = false;
            height = 0f;
            segmentNum = 0;
            width = 0;
            constraintSimulations = 0;
        }

        /// <summary>
        /// Override this so that you can properly get rid of any data that you do not need anymore after the Verlet Chain has been destroyed
        /// </summary>
        public virtual void OnDispose()
        {
        }

        /// <summary>
        /// Use this to properly setup the Verlet Chain
        /// </summary>
        /// <param name="startPoint">Where the verlet chain should be spawned at</param>
        public void Start(Vector2? startPoint = null)
        {
            if (FirstTick)
            {
                Spawn(startPoint);
                FirstTick = false;
            }
        }

        /// <summary>
        /// Override this so that you can have custom spawn logic for the chain (it all happens in one tick)
        /// </summary>
        /// <param name="startPoint">Where the verlet chain should be spawned at</param>
        public virtual void Spawn(Vector2? startPoint = null)
        {
            Vector2 ropeStartPoint = (startPoint == null ? Main.MouseWorld : (Vector2)startPoint) + new Vector2(0, height);
            for (int i = 0; i < segmentNum; i++)
            {
                segments.Add(new VerletSegment(ropeStartPoint));
                ropeStartPoint.Y -= height;
            }
        }

        /// <returns>Current Vanilla wind value</returns>
        public float GetWindValue() => Main.windSpeed + Main.windSpeedTemp;

        /// <returns>Current Vanilla wind value as a Vector2</returns>
        public Vector2 Wind() => new Vector2(GetWindValue(), 0);

        /// <summary>
        /// The gravity of the chain. Use VerletUtils for easy access for normal gravity and no gravity
        /// </summary>
        public virtual Vector2 ForceGravity => new Vector2(0, VerletUtils.normalGravity);

        /// <summary>
        /// Method to add any extra AI to the Verlet Chain
        /// </summary>
        /// <param name="segments">All the segments in this Verlet Chain</param>
        public virtual void ExtraBehaviour(ref List<VerletSegment> segments)
        {
        }

        /// <summary>
        /// Method to add more forces to all segments
        /// </summary>
        /// <param name="segment">Current segment being modified</param>
        public virtual void AddForces(ref VerletSegment segment)
        {
        }

        /// <summary>
        /// Use this to simulate the Verlet Chain
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < segments.Count; i++)
            {
                VerletSegment segment = segments[i];
                Vector2 velocity = segment.position - segment.oldPosition;
                segment.oldPosition = segment.position;
                segment.position += velocity;
                segment.position += ForceGravity;
                AddForces(ref segment);
                segment.center = new Vector2(segment.position.X + (width / 2f), segment.position.Y + (height / 2f));
                segments[i] = segment;
            }

            for (int i = 0; i < constraintSimulations; i++)
            {
                ApplyConstraints();
            }

            VerletSegment firstSegment = segments[0];
            StartAttachTo(ref firstSegment);
            segments[0] = firstSegment;

            VerletSegment lastOne = segments[segments.Count - 1];
            EndAttachTo(ref lastOne);
            segments[segments.Count - 1] = lastOne;

            ExtraBehaviour(ref segments);
        }

        /// <summary>
        /// Method to apply constraints
        /// </summary>
        public virtual void ApplyConstraints()
        {
            for (int i = 0; i < segments.Count - 1; i++)
            {
                VerletSegment firstSeg = segments[i];
                VerletSegment secondSeg = segments[i + 1];

                Vector2 pos = (firstSeg.position - secondSeg.position);
                float dist = (float)Math.Sqrt(pos.X * pos.X + pos.Y * pos.Y);
                float error = Math.Abs(dist - height);
                Vector2 changeDir = Vector2.Zero;

                if (dist > height)
                {
                    changeDir = firstSeg.position - secondSeg.position;
                    changeDir.Normalize();
                }
                else if (dist < height)
                {
                    changeDir = secondSeg.position - firstSeg.position;
                    changeDir.Normalize();
                }

                Vector2 changeAmount = changeDir * error;

                firstSeg.position -= changeAmount * 0.5f;
                segments[i] = firstSeg;
                secondSeg.position += changeAmount * 0.5f;
                segments[i + 1] = secondSeg;
            }
        }

        /// <returns>Returns all segment points to a List of Vector2's</returns>
        public List<Vector2> GetSegmentPoints()
        {
            List<Vector2> returnSegmentPositions = new List<Vector2>();
            for (int i = 0; i < segments.Count - 1; i++)
            {
                VerletSegment segment = segments[i];
                returnSegmentPositions.Add(segment.position);
            }
            return returnSegmentPositions;
        }

        /// <returns>The current Segment List</returns>
        public List<VerletSegment> GetSegments()
        {
            return segments;
        }

        /// <summary>
        /// Use this to set the current segment list to a new list
        /// </summary>
        /// <param name="segments">New segments to replace the old ones</param>
        public void SetSegments(List<VerletSegment> segments)
        {
            this.segments = segments;
        }

        /// <summary>
        /// Method to add new Segments to the Verlet Chain
        /// </summary>
        /// <param name="count">The amount to add</param>
        /// <param name="position">The position to add to</param>
        public void AddSegments(int count, Vector2 position)
        {
            count = Math.Abs(count);
            segmentNum += count;
            for (int i = 0; i < count; i++)
            {
                segments.Add(new VerletSegment(position));
            }
        }

        /// <summary>
        /// Method to remove Segments from the Verlet Chain
        /// </summary>
        /// <param name="count">The amount to remove</param>
        public void RemoveSegments(int count)
        {
            count = Math.Abs(count);
            if (segments.Count > count && segments.Count - count > 2)
            {
                segmentNum -= count;
                for (int i = 0; i < count; i++)
                {
                    segments.RemoveAt(segments.Count - 1);
                }
            }
            else
            {
                while(segments.Count > 2)
                {
                    segments.RemoveAt(segments.Count - 1);
                }
            }
        }

        public void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            for (int i = 0; i < segments.Count - 1; i++)
            {
                VerletSegment firstSeg = segments[i];
                VerletSegment secondSeg = segments[i + 1];

                Utils.PlotTileLine(firstSeg.center, secondSeg.center, width, new Utils.PerLinePoint(DelegateMethods.CutTiles));
            }
        }

        public bool? Colliding(Rectangle targetHitbox)
        {
            float num0 = 0f;
            for (int i = 0; i < segments.Count - 1; i++)
            {
                VerletSegment firstSeg = segments[i];
                VerletSegment secondSeg = segments[i + 1];

                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), firstSeg.center, secondSeg.center, width, ref num0))
                    return true;
            }
            return false;
        }

        public void DebugDraw(SpriteBatch spriteBatch, Color color)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                spriteBatch.Draw(
                    Main.magicPixel,
                    segments[i].center - Main.screenPosition,
                    new Rectangle(0, 0, 2, 2),
                    color,
                    0f,
                    new Vector2(0),
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color, Texture2D texture, bool overrideWidth = false)
        {
            if (!FirstTick)
            {
                if (texture == Main.magicPixel)
                {
                    for (int i = 0; i < segments.Count - 1; i++)
                    {
                        VerletSegment firstSegment = segments[i];
                        VerletSegment secondSegment = segments[i + 1];
                        Vector2 distToSeg = firstSegment.center - secondSegment.center;
                        float segRot = distToSeg.ToRotation() + (float)(Math.PI / 2f);
                        float distance = distToSeg.Length();

                        spriteBatch.Draw(
                            Main.magicPixel,
                            firstSegment.center - Main.screenPosition,
                            new Rectangle(0, 0, width, (int)distance + 1),
                            color,
                            segRot,
                            new Vector2(width / 2, 0),
                            1f,
                            SpriteEffects.None,
                            0f);
                    }
                }
                else
                {
                    float divideTextureBy = texture.Height;
                    divideTextureBy /= segments.Count;
                    float segmentCount = 0;
                    for (int i = 0; i < segments.Count - 1; i++)
                    {
                        VerletSegment firstSegment = segments[i];
                        VerletSegment secondSegment = segments[i + 1];
                        Vector2 distToSeg = firstSegment.center - secondSegment.center;
                        float segRot = distToSeg.ToRotation() + (float)(Math.PI / 2f);
                        float distance = distToSeg.Length();

                        spriteBatch.Draw(
                            texture,
                            firstSegment.center - Main.screenPosition,
                            new Rectangle(0, (int)segmentCount, overrideWidth ? width : texture.Width, (int)distance + 1),
                            color,
                            segRot,
                            new Vector2(width / 2, 0),
                            1f,
                            SpriteEffects.None,
                            0f);
                        segmentCount += divideTextureBy;
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture, bool overrideWidth = false)
        {
            if (!FirstTick)
            {
                if (texture == Main.magicPixel)
                {
                    for (int i = 0; i < segments.Count - 1; i++)
                    {
                        VerletSegment firstSegment = segments[i];
                        VerletSegment secondSegment = segments[i + 1];

                        Color color = Lighting.GetColor((int)(firstSegment.center.X / 16), (int)(firstSegment.center.Y / 16));

                        Vector2 distToSeg = firstSegment.center - secondSegment.center;
                        float segRot = distToSeg.ToRotation() + (float)(Math.PI / 2f);
                        float distance = distToSeg.Length();

                        spriteBatch.Draw(
                            Main.magicPixel,
                            firstSegment.center - Main.screenPosition,
                            new Rectangle(0, 0, width, (int)distance + 1),
                            color,
                            segRot,
                            new Vector2(width / 2, 0),
                            1f,
                            SpriteEffects.None,
                            0f);
                    }
                }
                else
                {
                    float divideTextureBy = texture.Height;
                    divideTextureBy /= segments.Count;
                    float segmentCount = 0;
                    for (int i = 0; i < segments.Count - 1; i++)
                    {
                        VerletSegment firstSegment = segments[i];
                        VerletSegment secondSegment = segments[i + 1];

                        Color color = Lighting.GetColor((int)(firstSegment.center.X / 16), (int)(firstSegment.center.Y / 16));

                        Vector2 distToSeg = firstSegment.center - secondSegment.center;
                        float segRot = distToSeg.ToRotation() + (float)(Math.PI / 2f);
                        float distance = distToSeg.Length();

                        spriteBatch.Draw(
                            texture,
                            firstSegment.center - Main.screenPosition,
                            new Rectangle(0, (int)segmentCount, overrideWidth ? width : texture.Width, (int)distance + 1),
                            color,
                            segRot,
                            new Vector2(width / 2, 0),
                            1f,
                            SpriteEffects.None,
                            0f);
                        segmentCount += divideTextureBy;
                    }
                }
            }
        }

        public class VerletUtils
        {
            public const float noGravity = 0.0000000000000000000000000000000000001f; //stupid but it works
            public const float normalGravity = 1.5f;
        }
    }
}