using SharpDX;

namespace GameManager
{
    public static class Vector2Extencion
    {
        public static Vector2 Up(this Vector2 vector)
        {
            return new Vector2(-1, 0);
        }
        
        public static Vector2 Down(this Vector2 vector)
        {
            return new Vector2(1, 0);
        }
        
        public static Vector2 Left(this Vector2 vector)
        {
            return new Vector2(0, -1);
        }
        
        public static Vector2 Right(this Vector2 vector)
        {
            return new Vector2(0, 1);
        }
    }
}