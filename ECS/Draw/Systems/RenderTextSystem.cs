using ECS.Draw.Component;
using ECS.Movement.Components;
using Leopotam.Ecs;
using SharpDX.Mathematics.Interop;

namespace ECS.Draw.Systems
{
    public class RenderTextSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TextDrawingComponent, TextComponent, TransformComponent> _textFilter;
        private readonly EcsFilter<RenderTargetComponent> _folterRenderTarget;

        public void Run()
        {
            foreach (var i in _textFilter)
            {
                ref var textDrawingComponent = ref _textFilter.Get1(i);
                ref var textComponent = ref _textFilter.Get2(i);
                ref var transformComponent = ref _textFilter.Get3(i);

                foreach (var j in _folterRenderTarget)
                {
                    ref var renderTargetComponent = ref _folterRenderTarget.Get1(j);

                    renderTargetComponent.RenderTarget.DrawText(
                        textComponent.Text, textDrawingComponent.TextFormat, new RawRectangleF(
                            transformComponent.Position.X, transformComponent.Position.Y,
                            transformComponent.Scale.X, transformComponent.Scale.Y),
                        textDrawingComponent.TextBrush);
                }
            }
        }
    }
}
