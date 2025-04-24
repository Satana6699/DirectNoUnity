using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Game.Gun.Components;
using ECS.Keyboard;
using ECS.Movement.Components;
using Leopotam.Ecs;
using Settings;
using SharpDX;

namespace ECS.Game.Gun.Systems
{
    public class GunPiuSystem : IEcsRunSystem
    {
        private readonly EcsFilter<InputComponent> _keyboardFilter;
        private readonly EcsFilter<GunComponent, TransformComponent> _gunFilter;
        private readonly EcsWorld _world;
        private readonly GameConfig _gameConfig;

        public void Run()
        {
            foreach (int i in _keyboardFilter)
            {
                ref var input = ref _keyboardFilter.Get1(i);

                if (input.IsMouseJustPressed())
                {
                    foreach (int j in _gunFilter)
                    {
                        ref var gun = ref _gunFilter.Get1(j);
                        ref var transform = ref _gunFilter.Get2(j);

                        var bulletEntity = _world.NewEntity();
                        bulletEntity.Get<BulletComponent>().Damage = _gameConfig.GunDamage;
                        bulletEntity.Get<TransformComponent>().Position = transform.Position;
                        bulletEntity.Get<TransformComponent>().Size = new Vector2(30, 30);
                        bulletEntity.Get<SpriteComponent>().SpritePath = DataPath.SpriteBulletPath;

                        Vector2 mousePos = new Vector2(input.X, input.Y);
                        Vector2 direction = mousePos - transform.Position;
                        if (direction.LengthSquared() > float.Epsilon)
                        {
                            direction.Normalize();
                        }
                        else
                        {
                            direction = Vector2.UnitX;
                        }
                        bulletEntity.Get<VelocityComponent>().Velocity = direction * gun.BulletSpeed;

                        bulletEntity.Get<SpriteColliderComponent>().SpritePath = DataPath.SpriteColliderPath;

                        ref var colliderComponent = ref bulletEntity.Get<ColliderComponent>();

                        colliderComponent.OffsetPosition = new Vector2(0, 0);
                        colliderComponent.Scale = new Vector2(30, 30);
                    }
                }
            }
        }
    }
}
