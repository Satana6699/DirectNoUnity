using ECS.MonsterFabric.Components;
using Leopotam.Ecs;

namespace ECS.MonsterFabric.Systems
{
    public class WaveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MonsterFabricComponent> _monsterFabricFilter;

        public void Run()
        {
            foreach (int i in _monsterFabricFilter)
            {

            }
        }
    }
}
