using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EcsSystems
{
    private readonly List<ISystem> _allSystems = new();
    private readonly List<IRunSystem> _runSystems = new();
    private readonly List<IInitSystem> _initSystems = new();

    private EcsWorld _world;

    public EcsSystems(EcsWorld world)
    {
        _world = world;
    }

    public async Task Init()
    {
        foreach (var system in _initSystems)
        {
            await system.Init(this);
        }
    }

    public void Run()
    {
        foreach (var system in _runSystems)
        {
            system.Run(this);
        }
    }

    public EcsSystems Register(ISystem system)
    {
        if (system == null)
            throw new Exception("Can't register null system.");
        
        _allSystems.Add(system);
        
        if (system is IInitSystem initSystem)
            _initSystems.Add(initSystem);
        
        if (system is IRunSystem runSystem)
            _runSystems.Add(runSystem);

        return this;
    }

    public EcsWorld GetWorld()
    {
        return _world;
    }

    public void Destroy()
    {
        _world = null;
    }
}