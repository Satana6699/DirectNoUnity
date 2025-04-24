using ECS.MonsterFabric.Components;
using Leopotam.Ecs;
using Settings;

namespace ECS.MonsterFabric.Systems
{
    public class DifficultyFactorWaveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MonsterFabricComponent, UpdateDifficultyComponent> _monsterFabricFilter;
        public void Run()
        {
            foreach (int i in _monsterFabricFilter)
            {
                ref var monsterFabric = ref _monsterFabricFilter.Get1(i);
                /*monsterFabric.EnemiesSpawnCooldown = 5f - monsterFabric.DifficultyFactor * 0.5f;
                monsterFabric.EnemiesCurrentSpawnCooldown = monsterFabric.EnemiesSpawnCooldown;
                if (monsterFabric.EnemiesSpawnCooldown <= 0.5f) monsterFabric.EnemiesSpawnCooldown = 0.5f;
                monsterFabric.WaveCooldown = monsterFabric.EnemiesSpawnCooldown * monsterFabric.EnemiesPerWave / 4;*/
                monsterFabric.WaveCooldown = monsterFabric.DifficultyFactor * 4f;
                monsterFabric.EnemiesPerWave = monsterFabric.DifficultyFactor * 2;
                monsterFabric.CurrentCooldown = monsterFabric.WaveCooldown;

                ref var entity = ref _monsterFabricFilter.GetEntity(i);
                entity.Del<UpdateDifficultyComponent>();
                entity.Get<ActivatedSpawnerEvent>();
            }
        }
    }
}
