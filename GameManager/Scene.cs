using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using GameManager.Components;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace GameManager
{
    public class Scene
    {
        private Dictionary<string, Bitmap> _bitmaps = new Dictionary<string, Bitmap>();
        private List< GameObject> _gameObjects = new List<GameObject>();
        
        public Bitmap GetBitmap(RenderTarget renderTarget, string file)
        {
            if (_bitmaps.TryGetValue(file, out Bitmap bitmap))
            {
                return bitmap;
            }
            else
            {
                bitmap = LoadFromFile(renderTarget, file);
                _bitmaps.Add(file, bitmap);
                return bitmap;
            }
        }

        public void UpdateGameObjects()
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.IsActive)
                    gameObject.UpdateComponents();
            }
        }
        
        public void AddGameObject(GameObject gameObject)
        {
            gameObject.SetScene(this);
            _gameObjects.Add(gameObject);
        }
        
        public void AddGameObjectRange(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetScene(this);
            }
            
            _gameObjects.AddRange(gameObjects);
        }

        public List<GameObject> GetGameObjects()
        {
            return _gameObjects;
        }
        
        private Bitmap LoadFromFile(RenderTarget renderTarget, string file)
        {
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                    }

                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }
        
        public void RemoveBitmap(string file)
        {
            if (_bitmaps.TryGetValue(file, out Bitmap bitmap))
            {
                // Освобождение памяти на GPU
                bitmap.Dispose();
                _bitmaps.Remove(file);
            }
        }

        public void Dispose()
        {
            _bitmaps.Clear();
        }

        public void DrawScene(RenderTarget renderTarget)
        {
            foreach (var gameObject in _gameObjects)
            {
                if (!gameObject.IsActive)
                {
                    continue;
                }
                
                var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                
                renderTarget.DrawBitmap(GetBitmap(renderTarget, spriteRenderer.SpritePath), new RawRectangleF(
                        gameObject.Transform.Position.X, gameObject.Transform.Position.Y, 
                        gameObject.Transform.Position.X + gameObject.Transform.Scale.X, 
                        gameObject.Transform.Position.Y + gameObject.Transform.Scale.Y), 
                    1.0f, BitmapInterpolationMode.Linear);
            }
        }
    }
}