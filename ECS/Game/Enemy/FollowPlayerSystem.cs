using System;
using ECS.Movement.Components;
using Leopotam.Ecs;
using SharpDX;

namespace ECS.Game.Enemy
{
    public class FollowPlayerSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ZombieComponent, TransformComponent, VelocityComponent> _zombieFilter;
        private readonly EcsFilter<PlayerComponent, TransformComponent> _playerFilter;

        private Random _random = new Random();

        public void Run()
        {
            foreach (int i in _zombieFilter)
            {
                ref var zombie = ref _zombieFilter.Get1(i);
                ref var transformZombie = ref _zombieFilter.Get2(i);
                ref var velocityZombie = ref _zombieFilter.Get3(i);

                foreach (int j in _playerFilter)
                {
                    ref var transformPlayer = ref _playerFilter.Get2(j);
                    ref var playerPos = ref transformPlayer.Position;
                    float pos = _random.NextFloat(-1, 1);

                    var direction = playerPos + pos - transformZombie.Position;
                    direction.Normalize();

                    velocityZombie.Velocity = direction * zombie.Speed;
                }
            }
        }
    }
}
