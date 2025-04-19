using ECS.Movement.Components;
using Leopotam.Ecs;

namespace ECS.Movement.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, MoveInputComponent, TransformComponent> _playerFilter;

        public void Run()
        {
            foreach (int i in _playerFilter)
            {
                ref var playerComp = ref _playerFilter.Get1(i);
                ref var moveInputComp = ref _playerFilter.Get2(i);
                ref var transformComp = ref _playerFilter.Get3(i);

                ref float speed = ref playerComp.Speed;
                ref var moveInput = ref moveInputComp.MoveInput;
                ref var position = ref transformComp.Position;
                position.X += moveInput.X * speed * Settings.Settings.DeltaTime;
                position.Y += moveInput.Y * speed * Settings.Settings.DeltaTime;
            }
        }
    }
}
