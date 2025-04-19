using ECS.Draw.Component;
using Leopotam.Ecs;

namespace ECS.Draw.Systems
{
    public class DisposeRenderTextSystem : IEcsDestroySystem
    {
        private readonly EcsFilter<TextDrawingComponent> _filter;

        public void Destroy()
        {
            foreach (var i in _filter)
            {
                ref var textDrawingComponent = ref _filter.Get1(i);

                textDrawingComponent.TextFormat?.Dispose();
                textDrawingComponent.TextBrush?.Dispose();
                textDrawingComponent.DirectWriteFactory?.Dispose();
            }
        }
    }
}
