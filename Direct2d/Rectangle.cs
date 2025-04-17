using SharpDX;

namespace Direct2d
{
    public class Rectangle
    {
        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public Rectangle()
        {
            Position = Vector2.Zero;
            Size = Vector2.Zero;
        }
        
        public Rectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
    }
}