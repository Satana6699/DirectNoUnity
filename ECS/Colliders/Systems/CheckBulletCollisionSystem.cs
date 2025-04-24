using ECS.Colliders.Components;
using ECS.Game.Enemy;
using ECS.Game.Gun.Components;
using ECS.Game.Health.Components;
using ECS.Movement.Components;
using Leopotam.Ecs;
using Settings;

namespace ECS.Colliders.Systems
{
    public class CheckBulletCollisionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ColliderComponent, TransformComponent> _collisionFilter;
        private readonly GameConfig _gameConfig;

        public void Run()
        {
            foreach (int i in _collisionFilter)
            {
                ref var colliderA = ref _collisionFilter.Get1(i);
                ref var transformA = ref _collisionFilter.Get2(i);
                ref var entityA = ref _collisionFilter.GetEntity(i);

                if (entityA.Has<StaticComponent>())
                {
                    continue;
                }

                for (int j = 0; j < _collisionFilter.GetEntitiesCount(); j++)
                {
                    if (i == j) continue;

                    ref var entityB = ref _collisionFilter.GetEntity(j);
                    if ((entityA.Has<BulletComponent>() && entityB.Has<BulletComponent>()) ||
                        (entityA.Has<BulletComponent>() && entityB.Has<PlayerComponent>()) ||
                        (entityA.Has<PlayerComponent>() && entityB.Has<BulletComponent>()))
                    {
                        continue;
                    }

                    ref var colliderB = ref _collisionFilter.Get1(j);
                    ref var transformB = ref _collisionFilter.Get2(j);

                    if (ColliderComponent.IsIntersecting(transformA.Position + colliderA.OffsetPosition, colliderA.Scale,
                            transformB.Position + colliderB.OffsetPosition, colliderB.Scale))
                    {
                        ZombieAndBulets(entityA, entityB);

                        ZombieAndPlayer(entityA, entityB);

                        Bullets(entityA, entityB);
                    }
                }
            }
        }

        private void ZombieAndPlayer(EcsEntity entityA, EcsEntity entityB)
        {
            if (entityA.Has<PlayerComponent>() && entityB.Has<ZombieComponent>())
            {
                entityA.Get<DamageEvent>().Value = _gameConfig.Zombie.Damage;
            }
            else if (entityB.Has<PlayerComponent>() && entityA.Has<ZombieComponent>())
            {
                entityB.Get<DamageEvent>().Value = _gameConfig.Zombie.Damage;
            }
        }

        private void ZombieAndBulets(EcsEntity entityA, EcsEntity entityB)
        {
            if (entityA.Has<ZombieComponent>() && entityB.Has<BulletComponent>())
            {
                entityA.Get<DamageEvent>().Value = entityB.Get<BulletComponent>().Damage;
            }
            else if (entityB.Has<ZombieComponent>() && entityA.Has<BulletComponent>())
            {
                entityB.Get<DamageEvent>().Value = entityA.Get<BulletComponent>().Damage;
            }
        }

        private void Bullets(EcsEntity entityA, EcsEntity entityB)
        {
            if (entityA.Has<BulletComponent>())
            {
                entityA.Destroy();
            }

            if (entityB.Has<BulletComponent>())
            {
                entityB.Destroy();
            }
        }
    }
}
