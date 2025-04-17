using System;
using System.Collections.Generic;
using System.Drawing;

class Program
{
    private const int MAX_LEAF_SIZE = 10;
    
    static void Main()
    {
        List<Leaf> leafs = new List<Leaf>();
        
        int mapWidth = 100, mapHeight = 100; 
        Leaf root = new Leaf(0, 0, mapWidth, mapHeight);
        leafs.Add(root);
        
        bool didSplit = true;
        Random random = new Random();
        
        while (didSplit)
        {
            didSplit = false;
            List<Leaf> newLeafs = new List<Leaf>();
            
            foreach (var l in leafs)
            {
                if (l.LeftChild == null && l.RightChild == null) // Если лист ещё не разрезан
                {
                    if (l.Width > MAX_LEAF_SIZE || l.Height > MAX_LEAF_SIZE || random.NextDouble() > 0.25)
                    {
                        if (l.Split())
                        {
                            newLeafs.Add(l.LeftChild);
                            newLeafs.Add(l.RightChild);
                            didSplit = true;
                        }
                    }
                }
            }
            
            leafs.AddRange(newLeafs);
        }
        
        GenerateBitmap(leafs, mapWidth, mapHeight);
    }
    
    static void GenerateBitmap(List<Leaf> leafs, int width, int height)
    {
        int coefizient = 6;
        
        using (Bitmap bmp = new Bitmap(width * coefizient + 1, height * coefizient + 1))
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.White);
            
            foreach (var leaf in leafs)
            {
                using (Brush blackBrush = new SolidBrush(Color.Black))
                using (Pen grayPen = new Pen(Color.Gray, 1))
                {
                    g.FillRectangle(blackBrush, leaf.X * coefizient, leaf.Y * coefizient, leaf.Width * coefizient, leaf.Height * coefizient);
                    g.DrawRectangle(grayPen, leaf.X * coefizient, leaf.Y * coefizient, leaf.Width * coefizient, leaf.Height * coefizient);
                }
            }
            
            bmp.Save("BSPMap.png");
            Console.WriteLine("BSPMap.png");
        }
    }
}
