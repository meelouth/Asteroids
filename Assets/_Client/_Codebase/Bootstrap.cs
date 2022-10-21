using UnityEngine;

namespace _Client
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Configuration configuration;
        [SerializeField] private SceneData sceneData;
        [SerializeField] private UIService uiService;

        [Header("Pools")]
        [SerializeField] private PoolContainer asteroidsPool;
        [SerializeField] private PoolContainer bulletPool;
        [SerializeField] private PoolContainer ufoPool;

        private ShipViewProvider _shipViewProvider;
        private EnvironmentSceneProvider _environmentSceneProvider;
        
        private StateMachine _stateMachine;
        
        private async void Start()
        {
            _stateMachine = new StateMachine();

            var world = new EcsWorld();
            var systems = new EcsSystems(world);
            
            var spawnService = new SpawnService();
            var asteroidsFactory = new AsteroidsFactoryService(asteroidsPool, configuration.AsteroidsConfiguration, world);
            var gunsFactory = new GunsFactory(world);
            
            _shipViewProvider = new ShipViewProvider();
            _environmentSceneProvider = new EnvironmentSceneProvider();

            systems
                .Register(new PlayerInitSystem(configuration.ShipConfiguration, _shipViewProvider, 
                    sceneData, gunsFactory))

                .Register(new PlayerInputSystem())

                .Register(new PlayerShipAccelerationSystem())
                .Register(new PlayerShipDecelerationSystem())
                .Register(new ShipsSpeedLimitationSystem())

                .Register(new VelocityMovementSystem())

                .Register(new PlayerShipRotationSystem())

                .Register(new ShipTeleportSystem(sceneData.Camera))

                .Register(new CollisionSystem<BulletView, Markers<HitByBullet, HitByPlayer>>())
                .Register(new CollisionSystem<AsteroidView, Markers<HitByDanger>>())
                .Register(new CollisionSystem<UFOView, Markers<HitByDanger>>())

                .Register(new GunsTriggerSystem())
                .Register(new BulletGunSystem(bulletPool))
                .Register(new BulletDestructionSystem())

                .Register(new LaserGunSystem())
                .Register(new LaserGunUIViewSystem(uiService))

                .Register(new OneFrameSystem<Triggered>())

                .Register(new AsteroidsSpawnSystem(configuration.AsteroidsConfiguration, asteroidsFactory, spawnService))
                .Register(new AsteroidsDestructionSystem(asteroidsFactory))

                .Register(new SpawnUFOSystem(ufoPool, configuration.UFOConfiguration, spawnService))
                .Register(new UFODestructionSystem())
                .Register(new FollowingSystem())

                .Register(new DisplayPlayerInformationSystem(uiService))

                .Register(new OneFrameSystem<Collision>())
                .Register(new OneFrameSystem<HitByBullet>())
                .Register(new OneFrameSystem<HitByPlayer>())

                .Register(new LifetimeSystem())

                .Register(new ScoreCountingSystem(configuration.PlayerScoringConfiguration))

                .Register(new ReturnToPoolSystem())
                .Register(new DestructionSystem())

                .Register(new PlayerLoseSystem(uiService, _stateMachine))
                
                .Register(new OneFrameSystem<HitByDanger>());

            var initializationState = new GameInitializationState(_stateMachine, systems, uiService, 
                    _environmentSceneProvider, asteroidsPool, bulletPool, ufoPool);
            
            _stateMachine.SwitchState(initializationState);
        }

        private void Update()
        {
            _stateMachine?.Run();
        }

        private void OnDestroy()
        {
            _stateMachine?.Destroy();
            
            _shipViewProvider?.Unload();
            _environmentSceneProvider?.Unload();

            ReleasePools();
        }

        private void ReleasePools()
        {
            asteroidsPool.Destroy();
            bulletPool.Destroy();
            ufoPool.Destroy();
        }
    }
}