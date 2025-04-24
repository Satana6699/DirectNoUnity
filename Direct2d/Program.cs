using System;
using SharpDX.Core;
using SharpDX.DirectInput;
using SharpDX.Mathematics.Interop;
using ECS;
using System.Runtime.InteropServices;
using Settings;

namespace Direct2d
{
    public class Program : Direct2D1DemoApp
    {
        private DirectInput _directInput;
        private Keyboard _keyboard;

        private MainWorld _maineWorld;

        // Мышь
        private Mouse _mouse;
        private int _mouseX = 0;
        private int _mouseY = 0;
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        // Кнопки
        private GameManager _gameManager;

        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);

            //========= Инициализация клавиатуры ================================
            _directInput = new DirectInput();
            _keyboard = new Keyboard(_directInput);
            _keyboard.Acquire();

            //========= Инициализация мыши ======================================
            _mouse = new Mouse(_directInput);
            _mouse.Acquire();

            //========= Создание кнопки =========================================

            _gameManager = new GameManager(RenderTarget2D, _keyboard, _mouse, Exit);
        }

        protected override void Update(DemoTime time)
        {
            base.Update(time);
            Time.DeltaTime = FrameDelta;

            GetCursorPos(out POINT point);
            ScreenToClient(GetActiveWindow(), ref point);

            _mouseX = point.X;
            _mouseY = point.Y;

            var simpleState = new MouseStateSimple
            {
                X = _mouseX,
                Y = _mouseY,
                LeftButton = _mouse.GetCurrentState().Buttons[0]
            };

            if (_gameManager.Keyboard != null &&
                _gameManager != null &&
                _gameManager.Keyboard.GetCurrentState().IsPressed(Key.Escape))
            {
                _gameManager.MaineWorld?.EndGame();
                _gameManager.MaineWorld = null;
                _gameManager.ExitShop();
            }

            foreach (var button in _gameManager.ButtonsMenu)
                button.Value.HandleClick(simpleState);

            _gameManager.MaineWorld?.GameRun();
        }

        protected override void Draw(DemoTime time)
        {
            base.Draw(time);
            RenderTarget2D.Clear(new RawColor4(0, 0, 0, 1));

            _gameManager.MaineWorld?.RenderRun();

            foreach (var button in _gameManager.ButtonsMenu)
                button.Value.Draw(RenderTarget2D);
        }

        protected override void Dispose(bool disposing)
        {
            _gameManager.MaineWorld?.EndGame();
            _gameManager.MaineWorld = null;

            base.Dispose(disposing);
        }

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("Рогалик"));
        }
    }
}
