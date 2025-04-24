using ECS.Colliders.Components;
using ECS.Movement.Components;
using Leopotam.Ecs;
using SharpDX;
using SharpDX.DirectInput;

namespace ECS.Keyboard
{
    public class KeyboardSystem : IEcsRunSystem
    {
        private readonly EcsFilter<InputComponent> _keyboardFilter;
        private readonly EcsFilter<MoveInputComponent> _moveFilter;
        private readonly EcsFilter<IsVisibleComponent> _isVisibleFilter;

        public void Run()
        {
            foreach (int i in _keyboardFilter)
            {
                ref var keyboardComponent = ref _keyboardFilter.Get1(i);

                keyboardComponent.UpdateInput();


                float horizontal = 0f;
                float vertical = 0f;

                // Управление игроком (WASD)
                if (keyboardComponent.CurrentKeyboardState.IsPressed(Key.W))
                    vertical = -1;
                if (keyboardComponent.CurrentKeyboardState.IsPressed(Key.S))
                    vertical = 1;
                if (keyboardComponent.CurrentKeyboardState.IsPressed(Key.A))
                    horizontal = -1;
                if (keyboardComponent.CurrentKeyboardState.IsPressed(Key.D))
                    horizontal = 1;

                Vector2 direction = new Vector2(horizontal, vertical);

                if (direction.LengthSquared() > 0)
                    direction = Vector2.Normalize(direction);

                foreach (int j in _moveFilter)
                {
                    ref var moveInputComponent = ref _moveFilter.Get1(j);

                    moveInputComponent.MoveInput = direction;
                }

                foreach (int j in _isVisibleFilter)
                {
                    ref var isVisibleComponent = ref _isVisibleFilter.Get1(j);

                    // Вкл/Выкл колайдеры
                    if (keyboardComponent.IsKeyJustPressed(Key.C))
                    {
                        isVisibleComponent.IsVisibleColliders = !isVisibleComponent.IsVisibleColliders;
                    }
                }
            }
        }
    }
}
