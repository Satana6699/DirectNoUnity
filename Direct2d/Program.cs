using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Core;
using SharpDX.Direct2D1.Effects;
using SharpDX.DirectInput;
using SharpDX.Mathematics.Interop;
using GameManager;
using GameManager.Components;
using GameSpace;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using Transform = GameManager.Components.Transform;

namespace Direct2d
{
    public class Program : Direct2D1DemoApp
    {
        private const float Speed = 100f;
        private DirectInput _directInput;
        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        
        private Scene _scene;
        private GameObject _player;
        
        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            _scene = new Scene();

            // Инициализация клавиатуры
            _directInput = new DirectInput();
            _keyboard = new Keyboard(_directInput);
            _keyboard.Acquire();
            
            //===================================================================================================
            // ДОБАВИТЬ СЛУЧАЙНЫЕ БЛОКИ НА КАРТУ ДЛЯ ТЕСТА
            Map map = new Map(40, 30, RenderTarget2D.PixelSize.Width, RenderTarget2D.PixelSize.Height);
            var gameObjectSpritePath = "block.png";
            Random random = new Random();
            List<GameObject> gameObjects = new List<GameObject>();

            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(0, map.MapRectangles.GetLength(0));
                int y = random.Next(0, map.MapRectangles.GetLength(1));

                var rect = map.MapRectangles[x, y];

                var gameObject = new GameObject(new Transform(rect.Position, rect.Size, new Vector2(1, 0)), gameObjectSpritePath);
                gameObject.AddComponent(new BoxCollider(Vector2.Zero, rect.Size));
                gameObjects.Add(gameObject);
            }

            _scene.AddGameObjectRange(gameObjects);
            //===================================================================================================

            //====================== INITIALIZE SETTINGS GAME ======================

            var settingsGame = new SettingGame()
            {
                SpeedPlayer = 300f,
                MovePlayerSound = "movePlayer.wav",
                BulletSpeed = 100f,
            };
            
            //====================== INITIALIZE BULLET PREFAB ======================
            var bulletSpritePath = "sharpdx.png";
            var bulletPrefab = new GameObject(
                new Transform(new Vector2(0, 0), new Vector2(30, 30), new Vector2(0, 0)), 
                bulletSpritePath);
            bulletPrefab.AddComponent<Bullet>().Construct(settingsGame);
            
            //====================== INITIALIZE GUN PREFAB ======================
            var gunSpritePath = "sharpdx.png";
            var gunPrefab = new GameObject(
                new Transform(new Vector2(0, 0), new Vector2(30, 30), new Vector2(0, 0)), 
                gunSpritePath);
            gunPrefab.AddComponent<Gun>().Construct(settingsGame, bulletPrefab);
            _scene.AddGameObject(gunPrefab);
            
            //====================== INITIALIZE PLAYER ======================
            var playerSpritePath = "sharpdx.png";
            _player = new GameObject(
                new Transform(new Vector2(100, 100), new Vector2(50, 50), new Vector2(0, 0)),
                playerSpritePath);
            _player.AddComponent(new Player()).Construct(settingsGame, gunPrefab);
            _scene.AddGameObject(_player);
        }

        protected override void Update(DemoTime time)
        {
            base.Update(time);
            Time.DeltaTime = FrameDelta;

            // Обновить состояние клавиатуры
            _keyboardState = _keyboard.GetCurrentState();
            if (_keyboardState == null)
                return;
            
            float horizontal = 0f;
            float vertical = 0f;
            
            // Управление игроком (WASD)
            if (_keyboardState.IsPressed(Key.W))
                vertical = -1;
            if (_keyboardState.IsPressed(Key.S))
                vertical = 1;
            if (_keyboardState.IsPressed(Key.A))
                horizontal = -1;
            if (_keyboardState.IsPressed(Key.D))
                horizontal = 1;

            
            var playerComponent = _player.GetComponent<Player>();
            
            if (playerComponent != null)
            {
                if (_keyboardState.IsPressed(Key.Space)) 
                    playerComponent.Fire();
                playerComponent.MoveInput(new Vector2(horizontal, vertical));
            }
            
            _scene.UpdateGameObjects();
        }


        protected override void Draw(DemoTime time)
        {
            base.Draw(time);
            // Черный фон
            RenderTarget2D.Clear(new RawColor4(0, 0, 0, 1));
            //Console.WriteLine($"RenderTarget2D.PixelSize: {RenderTarget2D.PixelSize}");
            
            _scene.DrawScene(RenderTarget2D);
        }

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("Рогалик"));
        }
    }
}
