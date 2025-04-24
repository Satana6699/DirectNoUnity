using ECS.Movement.Components;
using Leopotam.Ecs;

namespace ECS.Movement.Systems
{
    public class PlayerVelocitySystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, MoveInputComponent, VelocityComponent> _playerFilter;

        public void Run()
        {
            foreach (int i in _playerFilter)
            {
                ref var playerComp = ref _playerFilter.Get1(i);
                ref var moveInput = ref _playerFilter.Get2(i).MoveInput;
                ref var velocityComp = ref _playerFilter.Get3(i);

                ref float speed = ref playerComp.Speed;
                var velocity = moveInput * speed;
                velocityComp.Velocity = velocity;
            }
        }
    }
}
