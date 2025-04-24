namespace ECS.Game.Health.Components
{
    public struct HealthComponent
    {
        public float Value;
        private float _maxValue;

        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                Value = value;
            }
        }
    }
}
