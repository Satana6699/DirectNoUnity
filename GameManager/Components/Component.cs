namespace GameManager.Components
{ 
    public abstract class Component
    {
        public string Name {get; set;}
        public GameObject GameObject { get; set; }
        private static int _idCounter;
        public abstract Component Clone();

        static Component()
        {
            _idCounter = 0;
        }
        
        public Component()
        {
            Name = GetType().Name + _idCounter++;
        }
        
        public virtual void Initialize()
        {
            
        }
        
        public virtual void Update()
        {
            
        }

        public virtual void OnDestroy()
        {
            
        }

        public void Destroy(GameObject GameObject)
        {
            OnDestroy();
            // Destory
        }
    }
}