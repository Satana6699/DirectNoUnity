using ECS.Draw;
using ECS.Draw.Component;
using ECS.Movement.Components;
using ECS.UI.Components;
using Leopotam.Ecs;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using DWriteFactory = SharpDX.DirectWrite.Factory;

namespace ECS.Init
{

    public class InitDrawSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private RenderTarget _renderTarget;

        public InitDrawSystem(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public void Init()
        {
            _world.NewEntity().Get<RenderTargetComponent>().RenderTarget = _renderTarget;

            InitText();
        }

        private void InitText()
        {
            var entity = _world.NewEntity();
            ref var textComponent = ref entity.Get<TextDrawingComponent>();
            entity.Get<UiComponent>();
            textComponent.Font = "Arial";
            textComponent.DirectWriteFactory = new DWriteFactory();
            textComponent.TextFormat = new TextFormat(textComponent.DirectWriteFactory, textComponent.Font, 20f);
            textComponent.TextBrush = new SolidColorBrush(_renderTarget, new RawColor4(1f, 1f, 1f, 1f));

            entity.Get<TextComponent>().Text = "InitDrawSystem";
            ref var transformComponent = ref entity.Get<TransformComponent>();
            transformComponent.Position = new Vector2(10, 10);
            transformComponent.Size = new Vector2(900, 600);
        }
    }
}
