using ECS.Movement.Components;
using Leopotam.Ecs;
using SharpDX;

namespace ECS.Movement.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<VelocityComponent, TransformComponent> _playerFilter;

        public void Run()
        {
            foreach (int i in _playerFilter)
            {
                ref var velocityComp = ref _playerFilter.Get1(i);
                ref var transformComp = ref _playerFilter.Get2(i);

                ref var velocity = ref velocityComp.Velocity;

                if (velocity.LengthSquared() > 0)
                {
                    transformComp.ViewDirection = Vector2.Normalize(velocity);
                }

                if (_playerFilter.GetEntity(i).Has<PlayerComponent>())
                {
                    continue;
                }

                ref var position = ref transformComp.Position;
                position += velocity * Settings.Time.DeltaTime;
            }
        }
    }
}
