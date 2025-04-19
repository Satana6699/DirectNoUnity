using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using DWriteFactory = SharpDX.DirectWrite.Factory;

namespace ECS.Draw.Component
{
    public struct TextDrawingComponent
    {
        public string Font;
        public TextFormat TextFormat;
        public SolidColorBrush TextBrush;
        public DWriteFactory DirectWriteFactory;
    }
}
