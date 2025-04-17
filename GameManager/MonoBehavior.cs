using GameManager.Components;

namespace GameManager
{
    public abstract class MonoBehavior : Component
    {
        public override Component Clone()
        {
            return this.MemberwiseClone() as MonoBehavior;
        }
    }
}