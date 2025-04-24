using System;
using SharpDX;

namespace ECS.Colliders.Components
{
    public struct ColliderComponent
    {
        public Vector2 OffsetPosition;
        public Vector2 Scale;
        public bool IsVisible;

        public static bool IsIntersecting(Vector2 posA, Vector2 sizeA, Vector2 posB, Vector2 sizeB)
        {
            return posA.X < posB.X + sizeB.X &&
                   posA.X + sizeA.X > posB.X &&
                   posA.Y < posB.Y + sizeB.Y &&
                   posA.Y + sizeA.Y > posB.Y;
        }

        public static Vector2 GetPushOut(Vector2 posA, Vector2 sizeA, Vector2 posB, Vector2 sizeB)
        {
            float dx1 = (posB.X + sizeB.X) - posA.X;
            float dx2 = (posA.X + sizeA.X) - posB.X;
            float dy1 = (posB.Y + sizeB.Y) - posA.Y;
            float dy2 = (posA.Y + sizeA.Y) - posB.Y;

            float pushX = (dx1 < dx2) ? dx1 : -dx2;
            float pushY = (dy1 < dy2) ? dy1 : -dy2;

            // Выталкиваем по меньшей оси
            if (Math.Abs(pushX) < Math.Abs(pushY))
                return new Vector2(pushX, 0);
            else
                return new Vector2(0, pushY);
        }
    }
}
