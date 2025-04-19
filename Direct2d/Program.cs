using System;
using SharpDX.Core;
using SharpDX.DirectInput;
using SharpDX.Mathematics.Interop;
using ECS;
using ECS.Colliders.Components;
using ECS.Movement.Components;
using Leopotam.Ecs;
using SharpDX;

namespace Direct2d
{
    public class Program : Direct2D1DemoApp
    {
        private DirectInput _directInput;
        private Keyboard _keyboard;
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;

        private MainWorld _maineWorld;
        private EcsEntity _entity;
        private EcsEntity _isVisibleColliderEntity;


        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);

            //========================== ECS ==========================

            _maineWorld = new MainWorld();

            _entity = _maineWorld.World.NewEntity();
            _entity.Get<MoveInputComponent>().MoveInput = new Vector2(0, 0);

            _isVisibleColliderEntity = _maineWorld.World.NewEntity();
            _isVisibleColliderEntity.Get<IsVisibleComponent>().IsVisibleColliders = true;

            _maineWorld.GameInitSystems();
            _maineWorld.RenderInitSystems(RenderTarget2D);

            //=========================================================

            // Инициализация клавиатуры
            _directInput = new DirectInput();
            _keyboard = new Keyboard(_directInput);
            _keyboard.Acquire();
        }

        protected override void Update(DemoTime time)
        {
            base.Update(time);
            Settings.Settings.DeltaTime = FrameDelta;

            // Обновить состояние клавиатуры
            UpdateInput();


            float horizontal = 0f;
            float vertical = 0f;

            // Управление игроком (WASD)
            if (_currentKeyboardState.IsPressed(Key.W))
                vertical = -1;
            if (_currentKeyboardState.IsPressed(Key.S))
                vertical = 1;
            if (_currentKeyboardState.IsPressed(Key.A))
                horizontal = -1;
            if (_currentKeyboardState.IsPressed(Key.D))
                horizontal = 1;

            // Вкл/Выкл колайдеры
            if (IsKeyJustPressed(Key.C))
            {
                ref var isVisComp = ref _isVisibleColliderEntity.Get<IsVisibleComponent>();
                isVisComp.IsVisibleColliders = !isVisComp.IsVisibleColliders;
            }

            Vector2 direction = new Vector2(horizontal, vertical);

            if (direction.LengthSquared() > 0)
                direction = Vector2.Normalize(direction);

            ref var moveInputComponent = ref _entity.Get<MoveInputComponent>();
            moveInputComponent.MoveInput = direction;

            _maineWorld.GameRun();
        }
        protected override void Draw(DemoTime time)
        {
            base.Draw(time);
            RenderTarget2D.Clear(new RawColor4(0, 0, 0, 1));

            _maineWorld.RenderRun();
        }

        protected override void Dispose(bool disposing)
        {
            _maineWorld.Destroy();

            base.Dispose(disposing);
        }

        private void UpdateInput()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = _keyboard.GetCurrentState();

            if (_previousKeyboardState == null || _currentKeyboardState == null)
                return;
        }

        private bool IsKeyJustPressed(Key key)
        {
            return _currentKeyboardState.IsPressed(key) && !_previousKeyboardState.IsPressed(key);
        }

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("Рогалик"));
        }
    }
}
