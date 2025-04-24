using ECS.Draw.Component;
using ECS.Game.Enemy;
using ECS.Game.Health.Components;
using Leopotam.Ecs;
using Settings;

namespace ECS.Game.Health.Systems
{
    public class TakeDamageSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HealthComponent, DamageEvent> _healthFilter;

        public void Run()
        {
            foreach (int i in _healthFilter)
            {
                ref var health = ref _healthFilter.Get1(i);
                ref var damage = ref _healthFilter.Get2(i);
                ref var entity = ref _healthFilter.GetEntity(i);

                health.Value -= damage.Value;

                entity.Get<DamagedEvent>();

                if (entity.Has<ZombieComponent>())
                {
                    float healthPercent =  health.Value / health.MaxValue;

                    if (healthPercent < 0.75)
                    {
                        entity.Get<SpriteComponent>().SpritePath = DataPath.SpriteZombie1Path;
                        entity.Get<ZombieComponent>().Speed *= 0.8f;
                    }

                    if (healthPercent < 0.50)
                    {
                        entity.Get<SpriteComponent>().SpritePath = DataPath.SpriteZombie2Path;
                        entity.Get<ZombieComponent>().Speed *= 0.8f;
                    }

                    if (healthPercent < 0.25)
                    {
                        entity.Get<SpriteComponent>().SpritePath = DataPath.SpriteZombie3Path;
                        entity.Get<ZombieComponent>().Speed *= 0.8f;
                    }
                }

                if (health.Value <= 0)
                {
                    entity.Get<DeadEvent>();
                }

                entity.Del<DamageEvent>();
            }
        }
    }
}
