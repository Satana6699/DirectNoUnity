using System;
using System.Collections.Generic;
using System.Drawing;

public class Leaf
{
    private const int MIN_LEAF_SIZE = 15;

    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Leaf LeftChild { get; private set; }
    public Leaf RightChild { get; private set; }
    public Rectangle? Room { get; private set; }
    public List<Rectangle> Halls { get; private set; } = new List<Rectangle>();

    private static Random random = new Random();

    public Leaf(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public bool Split()
    {
        if (LeftChild != null || RightChild != null)
            return false; // Уже разрезан

        bool splitH = random.NextDouble() > 0.5;
        if (Width > Height && (float)Width / Height >= 1.25f)
            splitH = false;
        else if (Height > Width && (float)Height / Width >= 1.25f)
            splitH = true;

        int max = (splitH ? Height : Width) - MIN_LEAF_SIZE;
        if (max <= MIN_LEAF_SIZE)
            return false;

        int split = random.Next(MIN_LEAF_SIZE, max);

        if (splitH)
        {
            LeftChild = new Leaf(X, Y, Width, split);
            RightChild = new Leaf(X, Y + split, Width, Height - split);
        }
        else
        {
            LeftChild = new Leaf(X, Y, split, Height);
            RightChild = new Leaf(X + split, Y, Width - split, Height);
        }
        return true;
    }
}