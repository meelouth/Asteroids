using System.Collections;
using System.Collections.Generic;

public class EcsFilter : IEnumerable<Entity>
{
    private readonly HashSet<Entity> _entities = new();
    private readonly FilterMask _mask;

    public EcsFilter(FilterMask mask)
    {
        _mask = mask;
    }

    public void Register(Entity entity)
    {
        var isFilterMet = _mask.IsMet(entity);
        
        if (_entities.Contains(entity))
        {
            if (!isFilterMet)
            {
                _entities.Remove(entity);
            }

            return;
        }

        if (isFilterMet)
        {
            _entities.Add(entity);
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        _entities.Remove(entity);
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        var copy = new HashSet<Entity>(_entities);
        return copy.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}