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

        private ShipViewProvider shipViewProvider;
        private EnvironmentSceneProvider environmentSceneProvider;
        
        private StateMachine _stateMachine;
        
        private async void Start()
        {
            var world = new EcsWorld();
            var systems = new EcsSystems(world);
            _stateMachine = new StateMachine();
            
            var spawnService = new SpawnService();
            var asteroidsFactory = new AsteroidsFactoryService(asteroidsPool, configuration.AsteroidsConfiguration, world);
            var gunsFactory = new GunsFactory(world);
            
            shipViewProvider = new ShipViewProvider();
            environmentSceneProvider = new EnvironmentSceneProvider();

            systems
                .Register(new PlayerInitSystem(configuration.ShipConfiguration, shipViewProvider, 
                    sceneData, gunsFactory))

                .Register(new PlayerInputSystem())

                .Register(new PlayerShipAccelerationSystem())
                .Register(new PlayerShipDecelerationSystem())
                .Register(new ShipsSpeedLimitationSystem())

                .Register(new VelocityMovementSystem())

                .Register(new PlayerShipRotationSystem())

                .Register(new ShipTeleportSystem(sceneData))

                .Register(new CollisionSystem<BulletView, Markers<HitByBullet, HitByPlayer>>())
                .Register(new CollisionSystem<AsteroidView, Markers<HitByDanger>>())
                .Register(new CollisionSystem<UFOView, Markers<HitByDanger>>())

                .Register(new GunsTriggerSystem())
                .Register(new BulletGunSystem(bulletPool))
                .Register(new BulletDestructionSystem())

                .Register(new LaserGunSystem())
                .Register(new LaserGunUIViewSystem(uiService))

                .Register(new OneFrameSystem<Triggered>())

                .Register(
                    new AsteroidsSpawnSystem(configuration.AsteroidsConfiguration, asteroidsFactory, spawnService))
                .Register(new AsteroidsDestructionSystem(configuration.AsteroidsConfiguration, asteroidsFactory))

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
                    environmentSceneProvider, asteroidsPool, bulletPool, ufoPool);
            
            _stateMachine.SwitchState(initializationState);
        }

        private void Update()
        {
            _stateMachine?.Run();
        }

        private void OnDestroy()
        {
            _stateMachine?.Destroy();
            
            shipViewProvider?.Unload();
            environmentSceneProvider?.Unload();

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