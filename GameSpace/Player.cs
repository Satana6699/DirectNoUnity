using System;
using GameManager;
using GameManager.Components;
using SharpDX;

namespace GameSpace
{
    public class Player : MonoBehavior
    {
        private Vector2 _moveInput = Vector2.Zero;
        private float _moveSpeed = 0f;
        private AudioSource _audio;
        private GameObject _gunPrefab;
        private Gun _gun;
        
        public void Construct(SettingGame settingGame, GameObject gun)
        {
            _gunPrefab = gun;
            _gun = _gunPrefab.GetComponent<Gun>();
            
            _moveSpeed = settingGame.SpeedPlayer;
        }
        
        public override void Initialize()
        {
            GameObject.AddComponent(new BoxCollider(Vector2.Zero, GameObject.Transform.Scale));
            
            _audio = GameObject.AddComponent<AudioSource>();
        }

        public override void Update()
        {
            Move();

            UpdateGunPos();
            
            GameObject.Transform.Direction = _moveInput;
            GameObject.Transform.Direction.Normalize();
            
            _moveInput = Vector2.Zero;
        }

        private void UpdateGunPos()
        {
            if (_gun == null) 
                return;
            
            var offsetPosGun = 10f;
            _gun.GameObject.Transform.Position = GameObject.Transform.Position + GameObject.Transform.Direction * offsetPosGun;
            _gun.GameObject.Transform.Direction = GameObject.Transform.Direction;
        }
        
        public void Fire()
        {
            if (_gun == null) 
                return;
            
            _gun.Fire();
        }
        
        public void MoveInput(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }
        
        private void Move()
        {
            var movePosition = _moveInput * _moveSpeed * Time.DeltaTime;
            
            GameObject.Transform.Move(movePosition);
        }
    }
}