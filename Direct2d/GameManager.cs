using System;
using System.Collections.Generic;
using ECS;
using ECS.Keyboard;
using Leopotam.Ecs;
using Settings;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.Mathematics.Interop;

namespace Direct2d
{
    public class GameManager
    {
        public Dictionary<string, UIButton> ButtonsMenu = new Dictionary<string, UIButton>();
        public string[] menuItems = new string[] { "Start", "Exit", "RestartSavedData", "Shop" };
        public string[] shopItems = new string[] { "DamageUp", "HealthUp", "SpeedUp", "ExitShop" };
        public MainWorld MaineWorld;
        public Keyboard Keyboard;
        private RenderTarget _target;
        private Action _exitAction;
        private Mouse _mouse;
        private GameConfig _gameConfig;
        private ShopManager _shopManager = new ShopManager();

        public GameManager(RenderTarget target, Keyboard keyboard, Mouse mouse, Action exitAction)
        {
            _target = target;
            Keyboard = keyboard;
            _mouse = mouse;
            _exitAction = exitAction;
            _gameConfig = SerializeProgram.Load(DataPath.SaveDataGameConfig);

            // ДЛЯ ТЕСТОВ
            _gameConfig.Coins = 10000;

            InitMenuButtons(target);
            InitShopButtons(target);
            ExitShop();
        }

        private void InitMenuButtons(RenderTarget target)
        {
            (float X, float Y) size = (256, 64);
            (float X, float Y) position = (300, 100);
            (float X, float Y) padding = (0, 20);

            ButtonsMenu["Background"] = new UIButton(target,
                DataPath.Background, "",
                new RawRectangleF(0, 0, 900, 600), () => {});
            ButtonsMenu["Background"].SetHighlightEffect(false);

            ButtonsMenu["Start"] = new UIButton(target,
                DataPath.StartButton, "",
                new RawRectangleF(position.X,
                    position.Y + 0 * size.Y + 0 * padding.Y,
                    position.X + size.X,
                    position.Y + 1 * size.Y), Start);

            ButtonsMenu["Shop"] = new UIButton(target,
                DataPath.ShopButton, "Shop\n" + _gameConfig.Coins,
                new RawRectangleF(position.X,
                    position.Y + 1 * size.Y + 1 * padding.Y,
                    position.X + size.X,
                    position.Y + 1 * size.Y + 1 * padding.Y + size.Y), OpenShop);

            ButtonsMenu["RestartSavedData"] = new UIButton(target,
                DataPath.RestartButton, "Restart Saved Data",
                new RawRectangleF(position.X,
                    position.Y + 2 * size.Y + 2 * padding.Y,
                    position.X + size.X,
                    position.Y + 2 * size.Y + 2 * padding.Y + size.Y), RestartSavedData);

            ButtonsMenu["Exit"] = new UIButton(target,
                DataPath.ExitButton, "Exit",
                new RawRectangleF(position.X,
                    position.Y + 3 * size.Y + 3 * padding.Y,
                    position.X + size.X,
                    position.Y + 3 * size.Y + 3 * padding.Y + size.Y),
                () => _exitAction?.Invoke());
        }
        private void InitShopButtons(RenderTarget target)
        {
            (float X, float Y) size = (256, 64);
            (float X, float Y) position = (300, 100);
            (float X, float Y) padding = (0, 50);

            var damageValue = _shopManager.GetDamage(_gameConfig.PlayerDamageLevel);
            var healthValue = _shopManager.GetDamage(_gameConfig.PlayerHealthLevel);
            var speedValue = _shopManager.GetDamage(_gameConfig.PlayerSpeedLevel);

            ButtonsMenu["DamageUp"] = new UIButton(target,
                DataPath.Button, $"+ {damageValue.value} Damage\n{damageValue.coins} Coins",
                new RawRectangleF(position.X,
                    position.Y + 0 * size.Y + 0 * padding.Y,
                    position.X + size.X,
                    position.Y + 1 * size.Y), () =>
                {
                    var value = _shopManager.GetDamage(_gameConfig.PlayerDamageLevel);

                    if (_gameConfig.Coins < value.coins)
                        return;
                    _gameConfig.Coins -= value.coins;

                    _gameConfig.GunDamage += value.value;
                    _gameConfig.PlayerDamageLevel++;
                    value = _shopManager.GetDamage(_gameConfig.PlayerDamageLevel);
                    ButtonsMenu["DamageUp"].Text = $"+ {value.value} Damage\n{value.coins} Coins";
                });

            ButtonsMenu["HealthUp"] = new UIButton(target,
                DataPath.Button, $"+ {healthValue.value} Health\n{healthValue.coins} Coins",
                new RawRectangleF(position.X,
                    position.Y + 1 * size.Y + 1 * padding.Y,
                    position.X + size.X,
                    position.Y + 1 * size.Y + 1 * padding.Y + size.Y), () =>
                {
                    var value = _shopManager.GetHealth(_gameConfig.PlayerHealthLevel);

                    if (_gameConfig.Coins < value.coins)
                        return;
                    _gameConfig.Coins -= value.coins;

                    _gameConfig.Player.Health += value.value;
                    _gameConfig.PlayerHealthLevel++;
                    value = _shopManager.GetHealth(_gameConfig.PlayerHealthLevel);
                    ButtonsMenu["HealthUp"].Text = $"+ {value.value} Health\n{value.coins} Coins";
                });

            ButtonsMenu["SpeedUp"] = new UIButton(target,
                DataPath.Button, $"+ {speedValue.value} Speed\n{speedValue.coins} Coins",
                new RawRectangleF(position.X,
                    position.Y + 2 * size.Y + 2 * padding.Y,
                    position.X + size.X,
                    position.Y + 2 * size.Y + 2 * padding.Y + size.Y), () =>
                {
                    var value = _shopManager.GetSpeed(_gameConfig.PlayerSpeedLevel);

                    if (_gameConfig.Coins < value.coins)
                        return;
                    _gameConfig.Coins -= value.coins;

                    _gameConfig.Player.Speed += value.value;
                    _gameConfig.PlayerSpeedLevel++;
                    value = _shopManager.GetSpeed(_gameConfig.PlayerSpeedLevel);
                    ButtonsMenu["SpeedUp"].Text = $"+ {value.value} Speed\n{value.coins} Coins";
                });

            ButtonsMenu["ExitShop"] = new UIButton(target,
                DataPath.Button, "ExitShop",
                new RawRectangleF(position.X,
                    position.Y + 3 * size.Y + 3 * padding.Y,
                    position.X + size.X,
                    position.Y + 3 * size.Y + 3 * padding.Y + size.Y), ExitShop);
        }
        public void Start()
        {
            MaineWorld = new MainWorld(_gameConfig);

            var entityInput = MaineWorld.World.NewEntity();
            entityInput.Get<InputComponent>().Keyboard = Keyboard;
            entityInput.Get<InputComponent>().Mouse = _mouse;

            float width = _target.Size.Width;
            float height = _target.Size.Height;
            MaineWorld.RenderInitSystems(_target);
            MaineWorld.GameInitSystems(width, height);

            SetActiveAllButtons(false);
        }

        public void OpenShop()
        {
            var value = _shopManager.GetDamage(_gameConfig.PlayerDamageLevel);
            ButtonsMenu["DamageUp"].Text = $"+ {value.value} Damage\n{value.coins} Coins";

            value = _shopManager.GetHealth(_gameConfig.PlayerHealthLevel);
            ButtonsMenu["HealthUp"].Text = $"+ {value.value} Health\n{value.coins} Coins";

            value = _shopManager.GetSpeed(_gameConfig.PlayerSpeedLevel);
            ButtonsMenu["SpeedUp"].Text = $"+ {value.value} Speed\n{value.coins} Coins";

            SetActiveAllButtons(false, menuItems);
            SetActiveAllButtons(true, shopItems);
        }

        public void ExitShop()
        {
            ButtonsMenu["Shop"].Text = "Shop\n" + _gameConfig.Coins;
            SetActiveAllButtons(true, menuItems);
            SetActiveAllButtons(false, shopItems);
        }

        public void RestartSavedData()
        {
            var config = SerializeProgram.Load(DataPath.NewGameConfig);
            SerializeProgram.Save(config, DataPath.SaveDataGameConfig);
            _gameConfig = SerializeProgram.Load(DataPath.NewGameConfig);
        }

        public void SetActiveAllButtons(bool active)
        {
            foreach (var button in ButtonsMenu)
            {
                button.Value.SetActive(active);
            }
        }

        public void SetActiveAllButtons(bool active, string[] buttonNames)
        {
            foreach (string buttonName in buttonNames)
            {
                if (ButtonsMenu.ContainsKey(buttonName))
                {
                    ButtonsMenu[buttonName].SetActive(active);
                }
            }
        }
    }
}
