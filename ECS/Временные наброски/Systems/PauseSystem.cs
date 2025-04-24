using ECS.Keyboard;
using ECS.Временные_наброски.Components;
using Leopotam.Ecs;
using SharpDX.DirectInput;

namespace ECS.Временные_наброски.Systems
{
    public class PauseSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PauseComponent> _pauseFilter;
        private readonly EcsFilter<InputComponent> _keyboardFilter;

        public void Run()
        {
            foreach (int i in _pauseFilter)
            {
                ref var pauseComponent = ref _pauseFilter.Get1(i);

                foreach (int j in _keyboardFilter)
                {
                    ref var keyboardComponent = ref _keyboardFilter.Get1(j);

                    if (keyboardComponent.IsKeyJustPressed(Key.P))
                    {
                        pauseComponent.IsPause = !pauseComponent.IsPause;
                    }
                }
            }
        }
    }
}
