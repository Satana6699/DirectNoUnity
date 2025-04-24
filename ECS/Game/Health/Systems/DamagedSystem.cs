using ECS.Game.Health.Components;
using ECS.Movement.Components;
using ECS.UI.Components;
using Leopotam.Ecs;

namespace ECS.Game.Health.Systems
{
    public class DamagedSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent, DamagedEvent> _healthFilter;
        private readonly EcsFilter<PlayerHealthBar> _healthBarFilter;

        public void Run()
        {
            foreach (int i in _healthFilter)
            {
                ref var healthComponent = ref _healthFilter.Get1(i);

                if (!_healthFilter.GetEntity(i).Has<PlayerComponent>())
                    continue;

                foreach (int j in _healthBarFilter)
                {
                    ref var healthBar = ref _healthBarFilter.Get1(j);

                    healthBar.Fill = healthComponent.Value / healthComponent.MaxValue;
                }
            }
        }
    }
}
