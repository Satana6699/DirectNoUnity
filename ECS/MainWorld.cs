using ECS.Colliders.Systems;
using ECS.Draw;
using ECS.Draw.Systems;
using ECS.Init;
using ECS.Movement.Systems;
using Leopotam.Ecs;
using SharpDX.Direct2D1;

namespace ECS
{
    public class MainWorld
    {
        public readonly EcsWorld World;
        public readonly EcsSystems GameSystems;
        public readonly EcsSystems RenderSystems;

        public MainWorld()
        {
            World = new EcsWorld();
            GameSystems = new EcsSystems(World);
            RenderSystems = new EcsSystems(World);
        }

        public void GameInitSystems()
        {
            GameSystems.
                Add(new InitSystem()).
                Add(new InitCollidersSystem()).
                Add(new UpdateIsVisibleComponentsSystem()).
                Add(new MovementSystem()).
                Add(new CheckCollisionSystem()).
                Inject(this)
                .Init();
        }

        public void RenderInitSystems(RenderTarget RenderTarget2D)
        {
            RenderSystems.
                Add(new InitDrawSystem(RenderTarget2D)).
                Add(new RenderSystem()).
                Add(new RenderTextSystem()).
                Add(new DisposeRenderTextSystem()).
                Inject(this)
                .Init();
        }

        public void GameRun()
        {
            GameSystems.Run();
        }

        public void RenderRun()
        {
            RenderSystems.Run();
        }

        public void Destroy()
        {
            GameSystems.Destroy();
            World.Destroy();
        }
    }
}
