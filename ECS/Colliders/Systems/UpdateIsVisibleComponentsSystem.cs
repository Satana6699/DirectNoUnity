using ECS.Colliders.Components;
using Leopotam.Ecs;

namespace ECS.Colliders.Systems
{
    public class UpdateIsVisibleComponentsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<IsVisibleComponent> _isVisibleFilter;
        private readonly EcsFilter<ColliderComponent> _colliderFilter;

        public void Run()
        {
            foreach (int i in _isVisibleFilter)
            {
                ref var isVisible = ref _isVisibleFilter.Get1(i);

                foreach (int j in _colliderFilter)
                {
                    ref var collider = ref _colliderFilter.Get1(j);

                    collider.IsVisible = isVisible.IsVisibleColliders;
                }
            }
        }
    }
}
