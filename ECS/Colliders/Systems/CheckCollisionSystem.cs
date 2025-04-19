using System;
using ECS.Colliders.Components;
using ECS.Movement.Components;
using ECS.Временные_наброски.Components;
using Leopotam.Ecs;
using SharpDX;

namespace ECS.Colliders.Systems
{
    public class UpdateIsVisibleComponentsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<IsVisibleComponent> _isVisibleFilter;
        private readonly EcsFilter<ColliderComponent> _colliderFilter;

        public void Run()
        {
            foreach (int i in _isVisibleFilter)
            {
                ref var isVisible = ref _isVisibleFilter.Get1(i);

                foreach (int j in _colliderFilter)
                {
                    ref var collider = ref _colliderFilter.Get1(j);

                    collider.IsVisible = isVisible.IsVisibleColliders;
                }
            }
        }
    }

    public class CheckCollisionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ColliderComponent, TransformComponent> _collisionFilter;

        public void Run()
        {
            foreach (int i in _collisionFilter)
            {
                ref var colliderA = ref _collisionFilter.Get1(i);
                ref var transformA = ref _collisionFilter.Get2(i);
                ref var entity = ref _collisionFilter.GetEntity(i);

                if (entity.Has<StaticComponent>())
                {
                    continue;
                }


                for (int j = 0; j < _collisionFilter.GetEntitiesCount(); j++)
                {
                    if (i == j) continue;

                    ref var colliderB = ref _collisionFilter.Get1(j);
                    ref var transformB = ref _collisionFilter.Get2(j);

                    if (IsIntersecting(transformA.Position + colliderA.OffsetPosition, colliderA.Scale,
                            transformB.Position + colliderB.OffsetPosition, colliderB.Scale))
                    {
                        Vector2 push = GetPushOut(transformA.Position + colliderA.OffsetPosition, colliderA.Scale,
                            transformB.Position + colliderB.OffsetPosition, colliderB.Scale);
                        transformA.Position += push;
                    }
                }
            }
        }

        bool IsIntersecting(Vector2 posA, Vector2 sizeA, Vector2 posB, Vector2 sizeB)
        {
            return posA.X < posB.X + sizeB.X &&
                   posA.X + sizeA.X > posB.X &&
                   posA.Y < posB.Y + sizeB.Y &&
                   posA.Y + sizeA.Y > posB.Y;
        }

        Vector2 GetPushOut(Vector2 posA, Vector2 sizeA, Vector2 posB, Vector2 sizeB)
        {
            float dx1 = (posB.X + sizeB.X) - posA.X;
            float dx2 = (posA.X + sizeA.X) - posB.X;
            float dy1 = (posB.Y + sizeB.Y) - posA.Y;
            float dy2 = (posA.Y + sizeA.Y) - posB.Y;

            float pushX = (dx1 < dx2) ? dx1 : -dx2;
            float pushY = (dy1 < dy2) ? dy1 : -dy2;

            // Выталкиваем по меньшей оси
            if (Math.Abs(pushX) < Math.Abs(pushY))
                return new Vector2(pushX, 0);
            else
                return new Vector2(0, pushY);
        }
    }
}
