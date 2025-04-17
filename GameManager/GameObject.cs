using System;
using System.Collections.Generic;
using System.Linq;
using GameManager.Components;
using SharpDX;

namespace GameManager
{
    public class GameObject
    {
        public bool IsActive = true;
        private Dictionary<Type, List<Component>> _components = new Dictionary<Type, List<Component>>();
        public Transform Transform;
        
        public Scene Scene { get; private set; }
        
        public GameObject(Transform transform)
        {
            Transform = transform;
            Transform.GameObject = this;
        }

        private GameObject()
        {
            
        }
        
        public GameObject(Transform transform, string spritePath) : this(transform)
        {
            AddComponent(new SpriteRenderer(spritePath));
        }
        
        public void UpdateComponents()
        {
            foreach (var components in _components.Values)
            {
                foreach (var component in components)
                {
                    component.Update();
                }
            }
        }

        public T AddComponent<T>(T component) where T : Component
        {
            Type type = typeof(T);
            
            if (!_components.ContainsKey(type))
            {
                _components[type] = new List<Component>();
            }

            component.GameObject = this;
            component.Initialize();
            _components[type].Add(component);
            
            return component;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            Type type = component.GetType();
            
            if (!_components.ContainsKey(type))
            {
                _components[type] = new List<Component>();
            }

            component.GameObject = this;
            component.Initialize();
            _components[type].Add(component);
            
            return component;
        }
        
        public T GetComponent<T>() where T : Component
        {
            if (_components.TryGetValue(typeof(T), out List<Component> components) && components.Count > 0)
            {
                return components[0] as T;
            }

            return null;
        }
        
        public T GetComponent<T>(string nameComponent) where T : Component
        {
            if (_components.TryGetValue(typeof(T), out List<Component> components) && components.Count > 0)
            {
                foreach (Component component in components)
                {
                    if (component.Name == nameComponent)
                    {
                        return component as T;
                    }
                }
            }

            return null;
        }

        public List<T> GetComponents<T>() where T : Component
        {
            if (_components.TryGetValue(typeof(T), out List<Component> components))
            {
                return components.ConvertAll(c => c as T);
            }

            return new List<T>();
        }

        public bool RemoveComponent<T>() where T : Component
        {
            if (_components.TryGetValue(typeof(T), out List<Component> components) && components.Count > 0)
            {
                components.RemoveAt(0);
                if (components.Count == 0) _components.Remove(typeof(T));
                return true;
            }

            return false;
        }

        public bool RemoveAllComponents<T>() where T : Component
        {
            // Удалить все компоненты этого типа
            return _components.Remove(typeof(T));
        }

        public void SetScene(Scene scene)
        {
            Scene = scene;
        }
        
        public bool CheckCollision(GameObject other)
        {
            if (!other.IsActive || !IsActive)
            {
                return false;
            }
            
            BoxCollider thisCollider = GetComponent<BoxCollider>();
            BoxCollider otherCollider = other.GetComponent<BoxCollider>();

            if (thisCollider == null || otherCollider == null)
            {
                return false;
            }

            return thisCollider.IsColliding(otherCollider);
        }

        public void CorrectCollision(GameObject other, Vector2 direction)
        {
            BoxCollider thisCollider = GetComponent<BoxCollider>();
            BoxCollider otherCollider = other.GetComponent<BoxCollider>();

            if (thisCollider == null || otherCollider == null)
            {
                return;
            }

            // В каком направлении была коллизия
            Vector2 penetrationDepth = thisCollider.GetPenetrationDepth(otherCollider);

            // Смещание объекта ровно на величину пересечения, чтобы он вплотную прижался
            Transform.Position -= penetrationDepth;
        }

        public GameObject Instantiate(GameObject prefab, Transform transform)
        {
            // Новый GameObject
            var newObject = new GameObject(transform);

            // Клонируем компоненты
            foreach (var kvp in prefab._components)
            {
                foreach (var originalComponent in kvp.Value)
                {
                    // Предположим, что у Component есть метод Clone
                    var clonedComponent = originalComponent.Clone();
                    newObject.AddComponent(clonedComponent);
                }
            }

            // Добавляем в сцену
            newObject.SetScene(Scene);
            Scene.AddGameObject(newObject);

            return newObject;
        }

    }
}