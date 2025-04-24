using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Format = SharpDX.DXGI.Format;

namespace Direct2d
{
    public class UIButton
    {
        public RawRectangleF Rect;
        public string Text;
        public Action OnClick;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _textBrush;
        private TextFormat _textFormat;

        private bool _wasPressedLastFrame = false;
        private bool _isActive = true;
        private bool _isHovered = false;
        private bool _isPressedNow = false;
        private string _spritePath;
        private bool _enableHighlightEffect = true;

        public UIButton(RenderTarget renderTarget, string path, string text, RawRectangleF rect, Action onClick)
        {
            Rect = rect;
            Text = text;
            OnClick = onClick;
            _spritePath = path;

            _backgroundBrush = new SolidColorBrush(renderTarget, new RawColor4(0.3f, 0.3f, 0.3f, 1f));
            _textBrush = new SolidColorBrush(renderTarget, new RawColor4(1f, 1f, 1f, 1f));

            var factory = new SharpDX.DirectWrite.Factory();
            _textFormat = new TextFormat(factory, "Cascadia Code", 24)
            {
                TextAlignment = TextAlignment.Center,
                ParagraphAlignment = ParagraphAlignment.Center
            };
        }


        public void Draw(RenderTarget renderTarget)
        {
            if (!_isActive) return;

            float opacity = 1f;

            if (_enableHighlightEffect)
            {
                if (_isPressedNow)
                {
                    opacity = 0.4f;
                }
                else if (_isHovered)
                {
                    opacity = 0.65f;
                }
            }

            renderTarget.DrawBitmap(GetBitmap(renderTarget, _spritePath), Rect,
                opacity, BitmapInterpolationMode.Linear);

            renderTarget.DrawText(Text, _textFormat, Rect, _textBrush);
        }


        public void HandleClick(MouseStateSimple mouse)
        {
            if (!_isActive) return;

            bool isInside =
                mouse.X >= Rect.Left && mouse.X <= Rect.Right &&
                mouse.Y >= Rect.Top && mouse.Y <= Rect.Bottom;

            _isHovered = isInside;
            _isPressedNow = mouse.LeftButton && isInside;

            // Срабатывает только при отпускании кнопки над кнопкой
            if (!mouse.LeftButton && _wasPressedLastFrame && isInside)
            {
                OnClick?.Invoke();
            }

            _wasPressedLastFrame = mouse.LeftButton;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            if (!isActive)
            {
                _isPressedNow = false;
                _isHovered = false;
            }
        }

        private readonly Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();

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

        public void SetHighlightEffect(bool enable)
        {
            _enableHighlightEffect = enable;
        }
    }
}
