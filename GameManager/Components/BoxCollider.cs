using System;
using SharpDX;

namespace GameManager.Components
{
    public class BoxCollider : Component
    {
        public Vector2 Offset { get; set; } = Vector2.Zero;
        public Vector2 Position { get; private set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        
        public bool IsTrigger = false;
        
        private string _bitmapPath;

        public BoxCollider(Vector2 offset, Vector2 size) : base()
        {
            Offset = offset;
            Size = size;
        }

        public override void Initialize()
        {
            base.Initialize();
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            if (GameObject == null || GameObject.Transform == null)
            {
                return;
            }
            
            Position = GameObject.Transform.Position + Offset;
        }
        
        public override void Update() { }
        
        public void SetBitmap(string bitmapPath)
        {
            _bitmapPath = bitmapPath;
        }
        
        public bool IsColliding(BoxCollider other)
        {
            var result = (Position.X          < other.Position.X + other.Size.X &&
                          Position.X + Size.X > other.Position.X &&
                          Position.Y          < other.Position.Y + other.Size.Y &&
                          Position.Y + Size.Y > other.Position.Y);
            return result;
        }
        
        public Vector2 GetPenetrationDepth(BoxCollider other)
        {
            // Вычисляем правый нижний угол каждого коллайдера
            float thisRight = Position.X + Size.X;
            float thisBottom = Position.Y + Size.Y;
    
            float otherRight = other.Position.X + other.Size.X;
            float otherBottom = other.Position.Y + other.Size.Y;

            // Вычисление глубины пересечения по осям X и Y
            float xDepth = Math.Min(thisRight - other.Position.X, otherRight - Position.X);
            float yDepth = Math.Min(thisBottom - other.Position.Y, otherBottom - Position.Y);

            // Если по одной из осей нет пересечения (глубина отрицательна), то возвращаем (0, 0)
            if (xDepth < 0 || yDepth < 0)
                return Vector2.Zero;

            // Вычисление направления проникновения, с учётом ближайшей оси
            Vector2 thisCenter = new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
            Vector2 otherCenter = new Vector2(other.Position.X + other.Size.X / 2, other.Position.Y + other.Size.Y / 2);

            if (xDepth < yDepth)
            {
                return new Vector2(xDepth * Math.Sign(thisCenter.X - otherCenter.X), 0);
            }
            else
            {
                return new Vector2(0, yDepth * Math.Sign(thisCenter.Y - otherCenter.Y));
            }
        }

        public override Component Clone()
        {
            return new BoxCollider(Offset, Size);
        }
    }
}