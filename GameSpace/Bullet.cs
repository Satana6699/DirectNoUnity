using GameManager;
using GameManager.Components;

namespace GameSpace
{
    public class Bullet : MonoBehavior
    {
        private float _bulletSpeed = 5f;
        private float _bulletLifeTime = 2f;
        
        public override void Initialize()
        {
        }

        public void Construct(SettingGame settingGame)
        {
            _bulletSpeed = settingGame.BulletSpeed;
        }
        
        public override void Update()
        {
            Life();
            
            var moveVector = GameObject.Transform.Direction * _bulletSpeed * Time.DeltaTime;
            
            GameObject.Transform.Move(moveVector);
        }

        private void Life()
        {
            _bulletLifeTime -= Time.DeltaTime;

            if (_bulletLifeTime <= 0)
            {
                Destroy(GameObject);
            }
        }
    }
}