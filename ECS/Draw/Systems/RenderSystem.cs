using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Movement.Components;
using ECS.UI.Components;
using Leopotam.Ecs;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace ECS.Draw.Systems
{
    public class RenderSystem : IEcsRunSystem
    {
        private readonly Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();
        private readonly EcsFilter<SpriteComponent, TransformComponent> _filterSprite;
        private readonly EcsFilter<SpriteColliderComponent, ColliderComponent, TransformComponent> _colliderSpriteFilter;
        private readonly EcsFilter<RenderTargetComponent> _folterRenderTarget;
        private readonly EcsFilter<TransformComponent, PlayerHealthBar> _filterRectangle;

        public void Run()
        {
            foreach (int i in _folterRenderTarget)
            {
                ref var renderTargetComponent = ref _folterRenderTarget.Get1(i);

                foreach (int j in _filterSprite)
                {
                    ref var spriteComponent = ref _filterSprite.Get1(j);
                    ref var transformComponent = ref _filterSprite.Get2(j);

                    renderTargetComponent.RenderTarget.DrawBitmap(GetBitmap(
                            renderTargetComponent.RenderTarget, spriteComponent.SpritePath),
                        new RawRectangleF(
                            transformComponent.Position.X, transformComponent.Position.Y,
                            transformComponent.Position.X + transformComponent.Size.X,
                            transformComponent.Position.Y + transformComponent.Size.Y),
                        1.0f, BitmapInterpolationMode.Linear);
                }

                foreach (int j in _colliderSpriteFilter)
                {
                    ref var spriteComponent = ref _colliderSpriteFilter.Get1(j);
                    ref var colliderComponent = ref _colliderSpriteFilter.Get2(j);
                    ref var transformComponent = ref _colliderSpriteFilter.Get3(j);

                    if (!colliderComponent.IsVisible)
                    {
                        continue;
                    }

                    renderTargetComponent.RenderTarget.DrawBitmap(GetBitmap(
                            renderTargetComponent.RenderTarget, spriteComponent.SpritePath),
                        new RawRectangleF(
                            transformComponent.Position.X + colliderComponent.OffsetPosition.X,
                            transformComponent.Position.Y + colliderComponent.OffsetPosition.Y,
                            transformComponent.Position.X + colliderComponent.OffsetPosition.X + colliderComponent.Scale.X,
                            transformComponent.Position.Y + colliderComponent.OffsetPosition.Y + colliderComponent.Scale.Y),
                        1.0f, BitmapInterpolationMode.Linear);
                }

                foreach (int j in _filterRectangle)
                {
                    ref var transformComponent = ref _filterRectangle.Get1(j);
                    ref var colorComponent = ref _filterRectangle.Get2(j);

                    renderTargetComponent.RenderTarget.FillRectangle(
                        new RawRectangleF(
                            transformComponent.Position.X, transformComponent.Position.Y,
                            transformComponent.Position.X + transformComponent.Size.X * colorComponent.Fill,
                            transformComponent.Position.Y + transformComponent.Size.Y),
                        colorComponent.ColorBrush);
                }
            }
        }

        public Bitmap GetBitmap(RenderTarget renderTarget, string file)
        {
            if (_bitmapCache.TryGetValue(file, out Bitmap bitmap))
            {
                return bitmap;
            }
            else
            {
                bitmap = LoadFromFile(renderTarget, file);
                _bitmapCache.Add(file, bitmap);
                return bitmap;
            }
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
    }
}
