using ECS.Draw.Component;
using ECS.Movement.Components;
using ECS.UI.Components;
using Leopotam.Ecs;
using Settings;

namespace ECS.Временные_наброски.Systems
{
    public class MapMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, VelocityComponent> _playerFilter;
        private readonly EcsFilter<TransformComponent>.Exclude<UiComponent> _mapFilter;

        public void Run()
        {
            foreach (int i in _playerFilter)
            {
                ref var player = ref _playerFilter.Get1(i);
                ref var velocityComponent = ref _playerFilter.Get2(i);

                foreach (int j in _mapFilter)
                {
                    ref var transformComponent = ref _mapFilter.Get1(j);

                    if (_mapFilter.GetEntity(j).Has<PlayerComponent>())
                    {
                        continue;
                    }

                    transformComponent.Position += velocityComponent.Velocity * Time.DeltaTime * -1;
                }
            }
        }
    }
}
