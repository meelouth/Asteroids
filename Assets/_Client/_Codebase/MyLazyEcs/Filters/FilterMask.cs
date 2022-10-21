using System;
using System.Collections.Generic;

public class FilterMask
{
    private readonly List<Type> _incrementingComponents = new();
    private readonly List<Type> _exceptingComponent = new();
    
    private readonly EcsWorld _world;

    public FilterMask(EcsWorld world)
    {
        _world = world;
    }

    public EcsFilter Build()
    {
        var filter = new EcsFilter(this);
        _world.RegisterFilter(filter);

        return filter;
    }
    
    public FilterMask With<T>()
    {
        _incrementingComponents.Add(typeof(T));

        return this;
    }

    public FilterMask Except<T>()
    {
        _exceptingComponent.Add(typeof(T));

        return this;
    }

    public bool IsMet(Entity entity)
    {
        if (!IsEntityHasIncrementingComponents(entity))
            return false;

        if (IsEntityHasExceptingType(entity)) 
            return false;

        return true;
    }

    private bool IsEntityHasIncrementingComponents(Entity entity)
    {
        if (_incrementingComponents.Count <= 0)
            return false;
        
        foreach (var requiredType in _incrementingComponents)
        {
            if (!entity.HasComponent(requiredType))
                return false;
        }

        return true;
    }


    private bool IsEntityHasExceptingType(Entity entity)
    {
        foreach (var exceptingType in _exceptingComponent)
        {
            if (entity.HasComponent(exceptingType))
                return true;
        }

        return false;
    }
}