using System.Text;
using ECS.Draw.Component;
using ECS.MonsterFabric.Components;
using ECS.MonsterFabric.Systems;
using ECS.Movement.Components;
using Leopotam.Ecs;
using Settings;
using SharpDX.Mathematics.Interop;

namespace ECS.Draw.Systems
{
    public class RenderTextSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TextDrawingComponent, TextComponent, TransformComponent> _textFilter;
        private readonly EcsFilter<RenderTargetComponent> _folterRenderTarget;
        private readonly EcsFilter<MonsterFabricComponent> _fabricMonsterFilter;
        private readonly GameConfig _gameConfig;
        private readonly EcsWorld _world;
        private readonly StringBuilder _text = new StringBuilder("0");

        public void Run()
        {
            foreach (int i in _textFilter)
            {
                ref var textDrawingComponent = ref _textFilter.Get1(i);
                ref var textComponent = ref _textFilter.Get2(i);
                ref var transformComponent = ref _textFilter.Get3(i);

                foreach (int j in _folterRenderTarget)
                {
                    ref var renderTargetComponent = ref _folterRenderTarget.Get1(j);


                    foreach (int k in _fabricMonsterFilter)
                    {
                        ref var monsterComponent = ref _fabricMonsterFilter.Get1(k);

                        _text.Clear();
                        _text.Append("Время до следующей волны: ").Append(monsterComponent.CurrentCooldown);
                    }

                    EcsEntity[] entities = new EcsEntity[1];
                    _world.GetAllEntities(ref entities);
                    _text.Append("\nКоличество entity: ").Append(entities.Length);

                    _text.Append("\n Характеристики: ").Append(entities.Length);
                    _text.Append("\n Урон: ").Append(_gameConfig.GunDamage);
                    _text.Append("\n Скорость игрока: ").Append(_gameConfig.Player.Speed);
                    _text.Append("\n Колво Хп: ").Append(_gameConfig.Player.Health);
                    _text.Append("\n Колво Coins: ").Append(_gameConfig.Coins);

                    textComponent.Text = _text.ToString();

                    renderTargetComponent.RenderTarget.DrawText(
                        textComponent.Text, textDrawingComponent.TextFormat, new RawRectangleF(
                            transformComponent.Position.X, transformComponent.Position.Y,
                            transformComponent.Size.X, transformComponent.Size.Y),
                        textDrawingComponent.TextBrush);
                }
            }
        }
    }
}
