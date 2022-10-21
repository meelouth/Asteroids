using System;
using System.Collections.Generic;

public class Entity
{
    public event Action<Entity> OnComponentUpdate;
    
    private readonly int _id;
    private readonly Dictionary<Type, IComponent> _components = new();

    public Entity(int id)
    {
        _id = id;
    }

    public T AddComponent<T>() where T : IComponent, new()
    {
        var component = new T();
        
        _components.Add(typeof(T), component);

        OnComponentUpdate?.Invoke(this);

        return component;
    }
    
    public T GetOrAddComponent<T>() where T : IComponent, new ()
    {
        if (_components.TryGetValue(typeof(T), out var component))
            return (T)component;

        return AddComponent<T>();
    }

    public T GetComponent<T>() where T : IComponent
    {
        if (!HasComponent<T>())
        {
            throw new Exception($"Entity {_id} doesn't have {typeof(T).Name} component.");
        }
        
        var component = _components[typeof(T)];

        return (T)component;
    }
    
    public void RemoveComponent<T>() where T : IComponent
    {
        _components.Remove(typeof(T));

        OnComponentUpdate?.Invoke(this);
    }

    public bool HasComponent<T>() where T : IComponent
    {
        return HasComponent(typeof(T));
    }

    public bool HasComponent(Type component)
    {
        return _components.ContainsKey(component);
    }

    public int GetId()
    {
        return _id;
    }
}