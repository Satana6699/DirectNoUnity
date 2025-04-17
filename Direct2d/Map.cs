using System.Diagnostics.CodeAnalysis;
using SharpDX;

namespace Direct2d
{
    public class Map
    {
        private int _width;
        private int _height;
        private Vector2 _sizeRectangle;
        
        public Rectangle[,] MapRectangles { get; private set; }
        
        public Map(int width, int height, int windowWidth, int windowHeight)
        {
            _width = width;
            _height = height;
            
            _sizeRectangle = new Vector2(windowWidth / (float)width, windowHeight / (float)height);
            
            InitializeMap();
        }

        private void InitializeMap()
        {
            MapRectangles = new Rectangle[_height, _width];
            
            Vector2 currentPoint = Vector2.Zero; 
            
            for (int i = 0; i < MapRectangles.GetLength(0); i++)
            {
                for (int j = 0; j < MapRectangles.GetLength(1); j++)
                {
                    MapRectangles[i, j] = new Rectangle(currentPoint, _sizeRectangle);
                    currentPoint.X += _sizeRectangle.X;
                }

                currentPoint.X = 0;
                currentPoint.Y += _sizeRectangle.Y;
            }
        }
    }
}