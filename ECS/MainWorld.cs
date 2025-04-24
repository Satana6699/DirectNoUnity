using System;
using ECS.Colliders.Systems;
using ECS.Draw.Systems;
using ECS.Game.Enemy;
using ECS.Game.Gun.Systems;
using ECS.Game.Health;
using ECS.Game.Health.Components;
using ECS.Game.Health.Systems;
using ECS.Init;
using ECS.Keyboard;
using ECS.LevelLoader;
using ECS.MonsterFabric.Systems;
using ECS.Movement.Systems;
using ECS.Временные_наброски.Components;
using ECS.Временные_наброски.Systems;
using Leopotam.Ecs;
using Settings;
using SharpDX.Direct2D1;

namespace ECS
{
    public class MainWorld
    {
        public readonly EcsWorld World;
        private readonly EcsSystems _alwaysActiveSystems;  // Системы, работающие всегда
        private readonly EcsSystems _pausableGameSystems;  // Системы, которые можно ставить на паузу
        private readonly EcsSystems _renderSystems;

        private readonly EcsEntity _pauseEntity;
        private GameConfig _gameConfig;

        public MainWorld(GameConfig gameConfig)
        {
            World = new EcsWorld();
            _alwaysActiveSystems = new EcsSystems(World);
            _pausableGameSystems = new EcsSystems(World);
            _renderSystems = new EcsSystems(World);

            _pauseEntity = World.NewEntity();
            _pauseEntity.Get<PauseComponent>().IsPause = false;

            _gameConfig = gameConfig;
        }

        public void GameInitSystems(float width, float height)
        {
            _alwaysActiveSystems.
                Add(new InitSystem()).
                //Add(new InitLevelSystem(width, height)).//
                Add(new LevelGenerateSystem()).
                Add(new InitCollidersSystem()).
                Add(new KeyboardSystem()).
                Add(new MouseSystem()).
                Add(new UpdateIsVisibleComponentsSystem()).
                Add(new PauseSystem()).
                Inject(_gameConfig).
                Inject(this).
                Init();

            _pausableGameSystems.
                Add(new PlayerVelocitySystem()).
                Add(new FollowPlayerSystem()).
                Add(new MovementSystem()).
                Add(new GunPiuSystem()).
                Add(new CheckBulletCollisionSystem()).
                Add(new CheckCollisionSystem()).
                Add(new TakeDamageSystem()).
                Add(new DamagedSystem()).
                Add(new DeadSystem()).
                Add(new MapMoveSystem()).

                // Система спавна зомби
                Add(new DifficultyFactorWaveSystem()).
                Add(new TimerWaveSystem()).
                Add(new SpawnMonsterSystem()).
                Inject(_gameConfig).
                Inject(this).
                Init();
        }

        public void RenderInitSystems(RenderTarget RenderTarget2D)
        {
            _renderSystems.
                Add(new InitDrawSystem(RenderTarget2D)).
                Add(new RenderSystem()).
                Add(new RenderTextSystem()).
                Add(new DisposeRenderTextSystem()).
                Inject(_gameConfig).
                Inject(this)
                .Init();
        }

        public void GameRun()
        {
            _alwaysActiveSystems.Run();

            if (!_pauseEntity.Get<PauseComponent>().IsPause)
            {
                _pausableGameSystems.Run();
            }
        }

        public void RenderRun()
        {
            _renderSystems.Run();
        }

        public void EndGame()
        {
            SerializeProgram.Save(_gameConfig, DataPath.SaveDataGameConfig);

            Destroy();
        }

        public void Destroy()
        {
            _alwaysActiveSystems.Destroy();
            _pausableGameSystems.Destroy();
            _renderSystems.Destroy();

            World.Destroy();
        }
    }
}
