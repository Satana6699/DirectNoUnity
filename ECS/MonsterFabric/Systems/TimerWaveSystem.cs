using System.Runtime.InteropServices;
using ECS.MonsterFabric.Components;
using Leopotam.Ecs;
using Settings;

namespace ECS.MonsterFabric.Systems
{
    public class TimerWaveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MonsterFabricComponent> _monsterFabricFilter;

        public void Run()
        {
            foreach (int i in _monsterFabricFilter)
            {
                ref var monsterFabric = ref _monsterFabricFilter.Get1(i);

                monsterFabric.CurrentCooldown -= Time.DeltaTime;

                if (monsterFabric.CurrentCooldown <= 0)
                {
                    monsterFabric.DifficultyFactor++;
                    _monsterFabricFilter.GetEntity(i).Get<UpdateDifficultyComponent>();
                }
            }
        }
    }
}
