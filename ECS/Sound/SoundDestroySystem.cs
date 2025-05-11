using Leopotam.Ecs;

namespace ECS.Sound
{
    public class SoundDestroySystem : IEcsDestroySystem
    {
        private readonly EcsFilter<SoundComponent> _soundFilter;

        public void Destroy()
        {
            foreach (int i in _soundFilter)
            {
                ref var soundComponent = ref _soundFilter.Get1(i);

                soundComponent.MasteringVoice.Dispose();
                soundComponent.XAudio2.Dispose();
            }
        }
    }
}
