using GameManager;
using SharpDX;

namespace GameSpace
{
    public class Gun : MonoBehavior
    {
        private GameObject _bulletPrefab;
        private float _bulletSpeed = 5f;
        

        public void Construct(SettingGame settingGame, GameObject bulletPrefab)
        {
            _bulletPrefab = bulletPrefab;
            _bulletSpeed = settingGame.BulletSpeed;
        }
        
        public void Fire()
        {
            GameObject.Instantiate(_bulletPrefab, GameObject.Transform);
        }
    }
}