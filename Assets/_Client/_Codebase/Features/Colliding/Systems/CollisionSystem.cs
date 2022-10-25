using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class CollisionSystem<TView, TMarks> : IInitSystem, IRunSystem 
        where TView : MonoBehaviour
        where TMarks : IMarks, new()
    {
        private readonly IMarks marks = new TMarks();

        private EcsFilter _collided;
        
        public Task Init(EcsSystems systems)
        {
            _collided = systems
                .GetWorld()
                .Filter()
                .With<Collision>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _collided)
            {
                var collision = entity.GetComponent<Collision>();

                if (collision.Other.TryGetComponent<TView>(out _))
                {
                    marks.AddMarks(collision.From.GetEntity());
                }
            }
        }
    }
}