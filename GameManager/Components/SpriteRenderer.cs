namespace GameManager.Components
{
    public class SpriteRenderer : Component
    {
        public string SpritePath { get; set; }
        
        public override void Update() { }

        public SpriteRenderer() : base()
        {
            
        }

        public SpriteRenderer(string spritePath)
        {
            SpritePath = spritePath;
        }
        
        public override Component Clone()
        {
            return new SpriteRenderer(SpritePath);
        }

    }
}