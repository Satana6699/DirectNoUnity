using System;
using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Game.Enemy;
using ECS.Game.Health.Components;
using ECS.MonsterFabric.Components;
using ECS.Movement.Components;
using Leopotam.Ecs;
using Settings;
using SharpDX;

namespace ECS.MonsterFabric.Systems
{
    public class SpawnMonsterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MonsterFabricComponent, ActivatedSpawnerEvent> _monsterFabricFilter;
        private readonly EcsFilter<SpawnPointEnemyComponent, TransformComponent> _spawnPointFilter;
        private readonly EcsWorld _world;
        private readonly GameConfig _gameConfig;

        public void Run()
        {
            foreach (int i in _monsterFabricFilter)
            {
                ref var monsterFabric = ref _monsterFabricFilter.Get1(i);

                // надо создавать зомби в точках спавна расставленых заранее
                //CreateZombie(transform.Position, transform.Size);

                for (int j = 0; j < monsterFabric.EnemiesPerWave; j++)
                {
                    int randomPoint = new Random().Next(0, _spawnPointFilter.GetEntitiesCount());
                    ref var spawnPoint = ref _spawnPointFilter.Get2(randomPoint);
                    CreateZombie(spawnPoint.Position, spawnPoint.Size);
                }

                _monsterFabricFilter.GetEntity(i).Del<ActivatedSpawnerEvent>();

                /*monsterFabric.EnemiesCurrentSpawnCooldown -= Time.DeltaTime;
                if (monsterFabric.EnemiesCurrentSpawnCooldown <= 0)
                {


                    monsterFabric.EnemiesCurrentSpawnCooldown = monsterFabric.EnemiesCurrentSpawnCooldown;
                }*/
            }
        }

        private void CreateZombie(Vector2 position, Vector2 size)
        {
            var zombie = _world.NewEntity();
            ref var transformComponent = ref zombie.Get<TransformComponent>();

            transformComponent.Position = position;
            transformComponent.Size = size;

            zombie.Get<SpawnPointEnemyComponent>();

            ref var colliderComponent = ref zombie.Get<ColliderComponent>();

            colliderComponent.Scale = size / 2;
            colliderComponent.OffsetPosition = size / 4;

            zombie.Get<SpriteComponent>().SpritePath = DataPath.SpriteZombiePath;
            zombie.Get<SpriteColliderComponent>().SpritePath = DataPath.SpriteColliderPath;
            zombie.Get<ZombieComponent>().Speed = _gameConfig.Zombie.Speed;
            zombie.Get<ZombieComponent>().Coins = 1;
            zombie.Get<VelocityComponent>().Velocity = new Vector2(0, 0);
            zombie.Get<HealthComponent>().MaxValue = _gameConfig.Zombie.Health;
        }
    }
}
