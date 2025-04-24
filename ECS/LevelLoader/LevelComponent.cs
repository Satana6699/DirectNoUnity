using System;
using System.IO;
using SharpDX;

namespace ECS.LevelLoader
{
    public struct LevelComponent
    {
        public char[,] Level;
        public Vector2 TileSize;

        public static char[,] LoadLevel(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int height = lines.Length;
            int width = lines[0].Split(' ').Length;

            char[,] levelMap = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                string[] cells = lines[y].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < width; x++)
                {
                    levelMap[y, x] = cells[x][0];
                }
            }

            return levelMap;
        }
    }
}
