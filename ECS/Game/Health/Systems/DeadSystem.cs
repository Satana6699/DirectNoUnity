using ECS.Game.Enemy;
using ECS.Game.Health.Components;
using Leopotam.Ecs;
using Settings;

namespace ECS.Game.Health.Systems
{
    public class DeadSystem : IEcsRunSystem
    {
        private readonly EcsFilter<DeadEvent> _deadFilter;
        private GameConfig _gameConfig;

        public void Run()
        {
            foreach (int i in _deadFilter)
            {
                ref var entity = ref _deadFilter.GetEntity(i);

                if (entity.Has<ZombieComponent>())
                {
                    ref var zombie = ref entity.Get<ZombieComponent>();
                    _gameConfig.Coins += zombie.Coins;
                }

                entity.Destroy();
            }
        }
    }
}
