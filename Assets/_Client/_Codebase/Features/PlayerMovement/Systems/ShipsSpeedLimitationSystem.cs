using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class ShipsSpeedLimitationSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _ships;

        public async Task Init(EcsSystems systems)
        {
            _ships = systems
                .GetWorld()
                .Filter()
                .With<ShipMovement>().With<Velocity>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _ships)
            {
                var shipMovement = entity.GetComponent<ShipMovement>();
                var velocity = entity.GetComponent<Velocity>();
                velocity.Amount = Mathf.Clamp(velocity.Amount, 0, shipMovement.MaxSpeed);
            }
        }
    }
}