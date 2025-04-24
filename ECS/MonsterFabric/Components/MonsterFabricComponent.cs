namespace ECS.MonsterFabric.Components
{
    public struct MonsterFabricComponent
    {
        public int DifficultyFactor; // Сложность волн (Бесконечный прирост)
        public float WaveCooldown; // Время между волнами в секундах
        public int EnemiesPerWave; // Количество врагов в текущей волне
        //public float EnemiesSpawnCooldown; // Время спавна новых врагов
        //public float EnemiesCurrentSpawnCooldown; // Оставшееся время спавна новых врагов
        public float CurrentCooldown; // Оставшееся время до следующей волны
    }
}
