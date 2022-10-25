using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class FollowingSystem : IInitSystem, IRunSystem
    {
        private EcsFilter _followers;
        
        public Task Init(EcsSystems systems)
        {
            _followers = systems
                .GetWorld()
                .Filter()
                .With<TransformRef>().With<Following>().Build();
            
            return Task.CompletedTask;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _followers)
            {
                var follower = entity.GetComponent<Following>();
                var transform = entity.GetComponent<TransformRef>().Ref;

                var direction = transform.position - follower.Target.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * follower.RotationSpeed);
            }
        }
    }
}