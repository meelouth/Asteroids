using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class PlayerInitSystem : IInitSystem
    {
        private readonly ShipConfiguration _shipConfiguration;
        private readonly ShipViewProvider _shipViewProvider;
        private readonly SceneData _sceneData;
        private readonly GunsFactory _gunsFactory;

        private EcsWorld _world;

        public PlayerInitSystem(ShipConfiguration shipConfiguration, ShipViewProvider shipViewProvider,
            SceneData sceneData, GunsFactory gunsFactory)
        {
            _shipConfiguration = shipConfiguration;
            _shipViewProvider = shipViewProvider;
            _sceneData = sceneData;
            _gunsFactory = gunsFactory;
        }

        public async Task Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            
            var entity = systems
                .GetWorld()
                .CreateEntity();
            
            var ship = await CreateShip(entity);

            AddMovement(entity, ship);
            AddWallet(entity);
            AddPlayerComponents(entity);
            AddCollider(ship, entity);
            await AttachWeapons(entity, ship.View.Muzzle);
        }

        private void AddCollider(Ship ship, Entity entity)
        {
            var colliderView = ship.View.GetComponent<ColliderView>();
            colliderView.Init(entity, _world);
        }

        private async Task AttachWeapons(Entity entity, Transform muzzle)
        {
            var armory = entity.AddComponent<Armory>();

            var guns = _shipConfiguration.Guns;
            var primaryGun = await _gunsFactory.Create(guns.PrimaryGun, muzzle);
            var laserGunEntity = await _gunsFactory.Create(guns.SecondaryGun, muzzle);
            
            armory.PrimaryGun = primaryGun;
            armory.SecondaryGun = laserGunEntity;
        }

        private void AddPlayerComponents(Entity entity)
        {
            entity.AddComponent<PlayerInput>();
            entity.AddComponent<Player>();
        }

        private async Task<Ship> CreateShip(Entity entity)
        {
            var ship = entity.AddComponent<Ship>();
            
            ship.View = await _shipViewProvider.Load();

            return ship;
        }

        private void AddWallet(Entity entity)
        {
            entity.AddComponent<Wallet>();
        }
        
        private void AddMovement(Entity entity, Ship ship)
        {
            var transform = entity.AddComponent<TransformRef>();
            transform.Ref = ship.View.transform;
            transform.Ref.transform.position = _sceneData.PlayerSpawnPosition.position;
            
            var shipMovement = entity.AddComponent<ShipMovement>();
            shipMovement.AccelerationRate = _shipConfiguration.AccelerationRate;
            shipMovement.DecelerationRate = _shipConfiguration.DecelerationRate;
            shipMovement.MaxSpeed = _shipConfiguration.MaxSpeed;
            shipMovement.RotationSpeed = _shipConfiguration.RotationSpeed;
            
            entity.AddComponent<Velocity>();
        }
    }
}