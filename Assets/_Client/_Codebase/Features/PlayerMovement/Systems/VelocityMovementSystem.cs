using System;
using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class VelocityMovementSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _underVelocity;

        public async Task Init(EcsSystems systems)
        {
            _underVelocity = systems
                .GetWorld()
                .Filter()
                .With<Velocity>().With<TransformRef>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _underVelocity)
            {
                var velocity = entity.GetComponent<Velocity>();
                var transform = entity.GetComponent<TransformRef>().Ref;

                transform.position += transform.up * velocity.Amount * Time.deltaTime;
            }
        }
    }
}