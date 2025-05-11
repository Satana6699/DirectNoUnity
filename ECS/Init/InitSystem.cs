using System.Threading.Tasks;
using ECS.Colliders.Components;
using ECS.Draw.Component;
using ECS.Game.Gun.Components;
using ECS.MonsterFabric.Components;
using ECS.Movement.Components;
using ECS.Sound;
using ECS.UI.Components;
using Leopotam.Ecs;
using Settings;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpDX.XAudio2;

namespace ECS.Init
{
    public class InitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<MoveInputComponent> _moveFilter;
        private readonly EcsFilter<RenderTargetComponent> _renderTargetFilter;

        public void Init()
        {
            InitCore();
            InitHealthBar();
            InitSpawner();
            InitMusic();
        }

        private void InitMusic()
        {
            var entity = _world.NewEntity();
            ref var music = ref entity.Get<SoundComponent>();
            music.XAudio2 = new XAudio2();
            music.MasteringVoice = new MasteringVoice(music.XAudio2);
            music.FilePath = DataPath.BackgroundMusik;

            var xaudio = music.XAudio2;
            var filePath = music.FilePath;

            Task.Run( async () =>
            {
                await SoundComponent.PlaySoundFileAsync(xaudio, filePath, true);
            });
        }

        private void InitSpawner()
        {
            var entity = _world.NewEntity();
            ref var monsterFabricComponent = ref entity.Get<MonsterFabricComponent>();
            monsterFabricComponent.DifficultyFactor = 1;
            monsterFabricComponent.WaveCooldown = 5f;
            monsterFabricComponent.CurrentCooldown = 5f;
            monsterFabricComponent.EnemiesPerWave = 5;
        }

        private void InitCore()
        {
            var entity = _world.NewEntity();
            entity.Get<MoveInputComponent>().MoveInput = new Vector2(0, 0);

            var isVisibleColliderEntity = _world.NewEntity();
            isVisibleColliderEntity.Get<IsVisibleComponent>().IsVisibleColliders = true;
        }

        private void InitHealthBar()
        {
            var backgroundEntity = _world.NewEntity();
            var healthBarEntity = _world.NewEntity();

            ref var backgroundTransform = ref backgroundEntity.Get<TransformComponent>();
            backgroundTransform.Position = new Vector2(50, 50);
            backgroundTransform.Size = new Vector2(200, 50);

            ref var healthBarTransform = ref healthBarEntity.Get<TransformComponent>();
            healthBarTransform.Position = new Vector2(50, 50);
            healthBarTransform.Size = new Vector2(200, 50);

            foreach (int i in _renderTargetFilter)
            {
                ref var renderTargetComponent = ref _renderTargetFilter.Get1(i);
                /*ref var backgroundRectangleColorComponent = ref backgroundEntity.Get<PlayerHealthBar>();
                backgroundEntity.Get<UiComponent>();
                backgroundRectangleColorComponent.ColorBrush = new SolidColorBrush(
                    renderTargetComponent.RenderTarget, new RawColor4(1.0f, 0.0f, 0.0f, 1f));
                backgroundRectangleColorComponent.Fill = 1f;*/
                ref var healthBarRectangleColorComponent = ref healthBarEntity.Get<PlayerHealthBar>();
                healthBarEntity.Get<UiComponent>();
                healthBarRectangleColorComponent.ColorBrush = new SolidColorBrush(
                    renderTargetComponent.RenderTarget, new RawColor4(1.0f, 0.0f, 0.0f, 1f));
                healthBarRectangleColorComponent.Fill = 1f;
            }
        }
    }
}
