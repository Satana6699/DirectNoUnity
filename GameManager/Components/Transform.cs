using SharpDX;

namespace GameManager.Components
{
    public class Transform : Component
    {
        private BoxCollider _boxCollider;
        
        private Vector2 _position = Vector2.Zero;
        private Vector2 _direction = new Vector2(1, 0);
        public Vector2 Scale { get; set; }
        
        // Предыдущая позиция
        private Vector2 _previousPosition;

        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                if (_boxCollider == null && GameObject != null)
                {
                    _boxCollider = GameObject.GetComponent<BoxCollider>();
                }

                _boxCollider?.UpdatePosition();
            }
        }

        public Transform(Vector2 position, Vector2 scale, Vector2 direction) : base()
        {
            Position = position;
            Scale = scale;
            Direction = direction;
        }

        public override void Update()
        {
        }

        public void MoveTo(Vector2 motion)
        {
            GameObject.Transform.Position += motion;
        }

        public void Move(Vector2 motion)
        {
            MoveOne(new Vector2(0, motion.Y));
            MoveOne(new Vector2(motion.X, 0));
        }
        
        private void MoveOne(Vector2 motion)
        {
            _previousPosition = Position;

            Position += motion;

            foreach (var other in GameObject.Scene.GetGameObjects()) 
            {
                if (other == this.GameObject) continue;

                if (GameObject.CheckCollision(other))
                {
                    Position = _previousPosition;
                
                    // Cкорректировать позицию, чтобы объект прижался к препятствию
                    //GameObject.CorrectCollision(other, motion);
                    break;
                }
            }
        }
        
        public override Component Clone()
        {
            return new Transform(Position, Scale, Direction);
        }

    }
}