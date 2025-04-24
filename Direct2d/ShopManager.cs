namespace Direct2d
{
    public class ShopManager
    {
        public float StartSpeed = 10f;
        public float StartHealth = 10f;
        public float StartDamage = 10f;

        public int SpeedCostКоэфициент = 10;
        public int HealthCostКоэфициент = 10;
        public int DamageCostКоэфициент = 10;

        public float SpeedКоэфициентУлучшения = 3;
        public float HealthКоэфициентУлучшения = 3;
        public float DamageКоэфициентУлучшения = 1;

        public (float value, int coins) GetSpeed(int currentLevel)
        {
            float value = StartSpeed + currentLevel * SpeedКоэфициентУлучшения;
            int coins = SpeedCostКоэфициент + SpeedCostКоэфициент * currentLevel;

            return (value, coins);
        }

        public (float value, int coins) GetHealth(int currentLevel)
        {
            float value = StartHealth + currentLevel * HealthКоэфициентУлучшения;
            int coins = HealthCostКоэфициент + HealthCostКоэфициент * currentLevel;

            return (value, coins);
        }

        public (float value, int coins) GetDamage(int currentLevel)
        {
            float value = StartDamage + currentLevel * DamageКоэфициентУлучшения;
            int coins = HealthCostКоэфициент + HealthCostКоэфициент * currentLevel;

            return (value, coins);
        }
    }
}
