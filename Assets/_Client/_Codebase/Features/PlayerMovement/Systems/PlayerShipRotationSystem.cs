using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class PlayerShipRotationSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _playerShips;

        public Task Init(EcsSystems systems)
        {
            _playerShips = systems
                .GetWorld()
                .Filter()
                .With<ShipMovement>().With<PlayerInput>().With<TransformRef>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _playerShips)
            {
                var input = entity.GetComponent<PlayerInput>();
                var transform = entity.GetComponent<TransformRef>().Ref;
                var shipMovement = entity.GetComponent<ShipMovement>();
                
                transform.Rotate(0, 0, input.Rotation * shipMovement.RotationSpeed * Time.deltaTime);
            }
        }
    }
}