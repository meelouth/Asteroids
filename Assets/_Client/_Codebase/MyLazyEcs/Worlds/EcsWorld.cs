using System.Collections.Generic;

public class EcsWorld
{
    private readonly Dictionary<int, Entity> _entities = new();
    private int currentId;

    private readonly List<EcsFilter> _filters = new();

    public Entity CreateEntity()
    {
        var id = currentId++;
        
        var entity = new Entity(id);

        entity.OnComponentUpdate += OnEntityModified;

        _entities.Add(id, entity);

        FiltrateEntity(entity);
        
        return entity;
    }

    public FilterMask Filter()
    {
        var mask = new FilterMask(this);

        return mask;
    }

    public void RegisterFilter(EcsFilter filter)
    {
        _filters.Add(filter);
        
        foreach (var entity in _entities.Values)
        {
            filter.Register(entity);
        }
    }

    public void DeleteEntity(int id)
    {
        var entity = _entities[id];

        entity.OnComponentUpdate -= OnEntityModified;
        
        _entities.Remove(id);

        foreach (var filter in _filters)
        {
            filter.UnregisterEntity(entity);
        }
    }

    private void FiltrateEntity(Entity entity)
    {
        foreach (var filter in _filters)
        {
            filter.Register(entity);
        }
    }

    private void OnEntityModified(Entity entity)
    {
        FiltrateEntity(entity);
    }
}