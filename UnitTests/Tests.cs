using ECS.Movement.Components;
using ECS.Movement.Systems;
using Leopotam.Ecs;
using NUnit.Framework;
using SharpDX;

namespace UnitTests
{
    [TestFixture]
    public class MovementSystemsTests
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        [SetUp]
        public void Setup()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
        }

        [TearDown]
        public void Teardown()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }

        [Test]
        public void MovementSystem_UpdatesPosition_WhenVelocityApplied()
        {
            // Arrange
            var movementSystem = new MovementSystem();
            _systems.Add(movementSystem);

            var entity = _world.NewEntity();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var transform = ref entity.Get<TransformComponent>();

            velocity.Velocity = new Vector2(1, 0);
            transform.Position = Vector2.Zero;

            // Act
            _systems.Init();
            _systems.Run();

            // Assert
            Assert.AreNotEqual(Vector2.Zero, transform.Position);
            Assert.AreEqual(Vector2.UnitX, transform.ViewDirection);
        }

        [Test]
        public void MovementSystem_DoesNotMovePlayer_WhenHasPlayerComponent()
        {
            // Arrange
            var system = new PlayerVelocitySystem();
            _systems.Add(system);

            var entity = _world.NewEntity();
            ref var player = ref entity.Get<PlayerComponent>();
            ref var input = ref entity.Get<MoveInputComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();

            player.Speed = 5f;
            input.MoveInput = new Vector2(1, 0);

            // Act
            _systems.Init();
            _systems.Run();

            // Assert
            Assert.AreEqual(new Vector2(5, 0), velocity.Velocity);
        }

        [Test]
        public void PlayerVelocitySystem_SetsVelocity_FromMoveInputAndSpeed()
        {
            // Arrange
            var system = new PlayerVelocitySystem();
            _systems.Add(system);

            var entity = _world.NewEntity();
            ref var player = ref entity.Get<PlayerComponent>();
            ref var input = ref entity.Get<MoveInputComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();

            player.Speed = 5f;
            input.MoveInput = new Vector2(1, 0);

            // Act
            _systems.Init();
            _systems.Run();

            // Assert
            Assert.AreEqual(new Vector2(5, 0), velocity.Velocity);
        }

        [Test]
        public void PlayerVelocitySystem_DoesNothing_WhenNoMoveInput()
        {
            // Arrange
            var system = new PlayerVelocitySystem();
            _systems.Add(system);

            var entity = _world.NewEntity();
            ref var player = ref entity.Get<PlayerComponent>();
            ref var input = ref entity.Get<MoveInputComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();

            player.Speed = 5f;
            input.MoveInput = Vector2.Zero;
            velocity.Velocity = Vector2.One;

            // Act
            _systems.Init();
            _systems.Run();

            // Assert
            Assert.AreEqual(Vector2.Zero, velocity.Velocity);
        }

        [Test]
        public void MovementSystem_UpdatesViewDirection_OnlyWhenMoving()
        {
            // Arrange
            var movementSystem = new MovementSystem();
            _systems.Add(movementSystem);

            var entity = _world.NewEntity();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var transform = ref entity.Get<TransformComponent>();

            transform.ViewDirection = Vector2.UnitY;
            velocity.Velocity = Vector2.Zero;

            // Act
            _systems.Init();
            _systems.Run();

            // Assert
            Assert.AreEqual(Vector2.UnitY, transform.ViewDirection);

            // Act 2 - Apply movement
            velocity.Velocity = new Vector2(1, 1);
            _systems.Run();

            // Assert 2
            var expectedDirection = Vector2.Normalize(new Vector2(1, 1));
            Assert.AreEqual(expectedDirection, transform.ViewDirection);
        }
    }
}
