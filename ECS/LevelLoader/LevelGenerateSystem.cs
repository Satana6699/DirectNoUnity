using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Game.Enemy;
using ECS.Game.Gun.Components;
using ECS.Game.Health;
using ECS.Game.Health.Components;
using ECS.MonsterFabric.Components;
using ECS.Movement.Components;
using Leopotam.Ecs;
using Settings;
using SharpDX;

namespace ECS.LevelLoader
{
    public class LevelGenerateSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<MoveInputComponent> _moveFilter;
        private readonly GameConfig _gameConfig;

        public void Init()
        {
            char[,] levelMap = LevelComponent.LoadLevel(DataPath.SpriteLevelsPath + @"\lvl1.txt");

            int levelMapHeight = levelMap.GetLength(0);
            int levelMapWidth = levelMap.GetLength(1);

            float tileHeight = 1000f;
            float tileWidth = 1000f;

            var tileSize = new Vector2(tileWidth, tileHeight);
            float posY = -1200f;

            for (int y = 0; y < levelMapHeight; y++)
            {
                float posX = -1200;
                for (int x = 0; x < levelMapWidth; x++)
                {
                    var tilePos = new Vector2(posX, posY);
                    char tile = levelMap[y, x];
                    CreateGrass(tilePos, tileSize);
                    switch (tile)
                    {
                        case 'b':
                            // Стена
                            CreateBlock(tilePos, tileSize, DataPath.SpriteBotBlockPath);
                            break;
                        case 't':
                            // Стена
                            CreateBlock(tilePos, tileSize, DataPath.SpriteTopBlockPath);
                            break;
                        case 'r':
                            // Стена
                            CreateBlock(tilePos, tileSize, DataPath.SpriteRightBlockPath);
                            break;
                        case 'l':
                            // Стена
                            CreateBlock(tilePos, tileSize, DataPath.SpriteLeftBlockPath);
                            break;
                        case 'z':
                            // Точка спавна зомби
                            CreateZombieFabric(tilePos, tileSize);
                            break;
                        default:
                            break;
                    }
                    posX += tileWidth;
                }
                posY += tileHeight;
            }

            var playerPos = new Vector2(420, 320);
            var playerSize = new Vector2(50, 50);
            InitPlayer(playerPos, playerSize);
        }

        private void CreateGrass(Vector2 position, Vector2 size)
        {
            var block = _world.NewEntity();
            ref var transformComponent = ref block.Get<TransformComponent>();

            transformComponent.Position = position;
            transformComponent.Size = size;

            block.Get<SpriteComponent>().SpritePath = DataPath.SpriteGrassPath;
        }

        private void CreateBlock(Vector2 blockPosition, Vector2 blockSize, string blockName)
        {
            var block = _world.NewEntity();
            ref var transformComponent = ref block.Get<TransformComponent>();

            transformComponent.Position = blockPosition;
            transformComponent.Size = blockSize;

            ref var colliderComponent = ref block.Get<ColliderComponent>();

            colliderComponent.OffsetPosition = new Vector2(0, 0);
            colliderComponent.Scale = blockSize;

            block.Get<SpriteComponent>().SpritePath = blockName;

            block.Get<StaticComponent>();
        }

        private void InitPlayer(Vector2 playerPosition, Vector2 playerSize)
        {
            foreach (int i in _moveFilter)
            {
                ref var entityPlayer = ref _moveFilter.GetEntity(i);
                ref var transformComponent = ref entityPlayer.Get<TransformComponent>();

                transformComponent.Position = playerPosition;
                transformComponent.Size = playerSize;
                transformComponent.ViewDirection = new Vector2(1, 0);

                ref var colliderComponent = ref entityPlayer.Get<ColliderComponent>();

                colliderComponent.OffsetPosition = new Vector2(0, 0);
                colliderComponent.Scale = playerSize;

                entityPlayer.Get<SpriteComponent>().SpritePath = DataPath.SpritePlayerGunPath;

                entityPlayer.Get<PlayerComponent>().Speed = _gameConfig.Player.Speed;
                entityPlayer.Get<GunComponent>().BulletSpeed = _gameConfig.BulletSpeed;
                entityPlayer.Get<VelocityComponent>().Velocity = new Vector2(0, 0);
                entityPlayer.Get<HealthComponent>().MaxValue = _gameConfig.Player.Health;
            }
        }

        private void CreateZombieFabric(Vector2 position, Vector2 size)
        {
            var zombie = _world.NewEntity();
            ref var transformComponent = ref zombie.Get<TransformComponent>();

            transformComponent.Position = position + size / 2;
            transformComponent.Size = new Vector2(50, 50);

            zombie.Get<SpawnPointEnemyComponent>();
        }
    }
}
