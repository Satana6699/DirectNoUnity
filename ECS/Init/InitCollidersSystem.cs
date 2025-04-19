using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Временные_наброски.Components;
using Leopotam.Ecs;

namespace ECS.Init
{
    public class InitCollidersSystem : IEcsInitSystem
    {
        private readonly EcsFilter<ColliderComponent> _filter;

        public void Init()
        {
            foreach (int i in _filter)
            {
                ref var collider = ref _filter.Get1(i);
                ref var entity = ref _filter.GetEntity(i);

                collider.IsVisible = true;
                entity.Get<SpriteColliderComponent>().SpritePath = "Sprites/collider.png";
            }
        }
    }
}
