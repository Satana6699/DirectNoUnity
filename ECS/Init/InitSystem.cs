using ECS.Colliders.Components;
using ECS.Draw;
using ECS.Draw.Component;
using ECS.Movement.Components;
using ECS.Временные_наброски.Components;
using Leopotam.Ecs;
using SharpDX;

namespace ECS.Init
{
    public class InitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<MoveInputComponent> _moveFilter;

        public void Init()
        {
            InitPlayer();
            InitBlocks();
        }

        private void InitBlocks()
        {
            var block = _world.NewEntity();
            ref var transformComponent = ref block.Get<TransformComponent>();

            transformComponent.Position = new Vector2(400, 200);
            transformComponent.Scale = new Vector2(100, 100);

            ref var colliderComponent = ref block.Get<ColliderComponent>();

            colliderComponent.OffsetPosition = new Vector2(0, 0);
            colliderComponent.Scale = new Vector2(100, 100);

            string blockSpritePath = "Sprites/block.png";
            block.Get<SpriteComponent>().SpritePath = blockSpritePath;

            block.Get<StaticComponent>();
        }

        private void InitPlayer()
        {
            foreach (int i in _moveFilter)
            {
                ref var entityPlayer = ref _moveFilter.GetEntity(i);
                ref var transformComponent = ref entityPlayer.Get<TransformComponent>();

                transformComponent.Position = new Vector2(200, 200);
                transformComponent.Scale = new Vector2(100, 100);

                ref var colliderComponent = ref entityPlayer.Get<ColliderComponent>();

                colliderComponent.OffsetPosition = new Vector2(0, 0);
                colliderComponent.Scale = new Vector2(100, 100);

                string playerSpritePath = "Sprites/Player.png";
                entityPlayer.Get<SpriteComponent>().SpritePath = playerSpritePath;

                entityPlayer.Get<PlayerComponent>().Speed = 100f;
            }
        }
    }
}
