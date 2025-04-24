using System;
using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Game.Enemy;
using ECS.Game.Gun.Components;
using ECS.Movement.Components;
using ECS.UI.Components;
using Leopotam.Ecs;
using SharpDX;

namespace ECS.Colliders.Systems
{
    public class CheckCollisionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ColliderComponent, TransformComponent> _collisionFilter;
        private readonly EcsFilter<PlayerComponent, VelocityComponent> _playerFilter;
        private readonly EcsFilter<TransformComponent>.Exclude<PlayerHealthBar, TextDrawingComponent> _mapFilter;

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
                        (entityA.Has<PlayerComponent>() && entityB.Has<BulletComponent>()) ||
                        (entityA.Has<PlayerComponent>() && entityB.Has<ZombieComponent>()) ||
                        (entityA.Has<ZombieComponent>() && entityB.Has<PlayerComponent>()) /*||
                        (entityA.Has<ZombieComponent>() && entityB.Has<ZombieComponent>())*/
                       )
                    {
                        continue;
                    }

                    ref var colliderB = ref _collisionFilter.Get1(j);
                    ref var transformB = ref _collisionFilter.Get2(j);

                    if (ColliderComponent.IsIntersecting(transformA.Position + colliderA.OffsetPosition, colliderA.Scale,
                            transformB.Position + colliderB.OffsetPosition, colliderB.Scale))
                    {
                        Vector2 push = ColliderComponent.GetPushOut(transformA.Position + colliderA.OffsetPosition, colliderA.Scale,
                            transformB.Position + colliderB.OffsetPosition, colliderB.Scale);

                        if (entityA.Has<PlayerComponent>())
                        {
                            PushMap(push);
                            continue;
                        }
                        else if (entityB.Has<PlayerComponent>())
                        {
                            PushMap(push);
                            continue;
                        }

                        transformA.Position += push;
                    }
                }
            }
        }

        private void PushMap(Vector2 push)
        {
            foreach (int i in _mapFilter)
            {
                ref var transform = ref _mapFilter.Get1(i);

                if (_mapFilter.GetEntity(i).Has<PlayerComponent>())
                {
                    continue;
                }

                transform.Position += -push;
            }
        }
    }
}
