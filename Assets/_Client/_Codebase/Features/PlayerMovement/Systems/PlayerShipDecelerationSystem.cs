using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class PlayerShipDecelerationSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _playerShips;

        public Task Init(EcsSystems systems)
        {
            _playerShips = systems
                .GetWorld()
                .Filter()
                .With<ShipMovement>().With<PlayerInput>().With<Velocity>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _playerShips)
            {
                var input = entity.GetComponent<PlayerInput>();

                if (input.IsMoving)
                    continue;

                var velocity = entity.GetComponent<Velocity>();
                var shipMovement = entity.GetComponent<ShipMovement>();

                velocity.Amount -= shipMovement.DecelerationRate * Time.deltaTime;
            }
        }
    }
}