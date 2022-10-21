using System.Threading.Tasks;
using UnityEngine;

namespace _Client
{
    public class ShipTeleportSystem : IInitSystem, IRunSystem
    {
        private readonly Camera _camera;

        private EcsFilter _ships;

        public ShipTeleportSystem(Camera camera)
        {
            _camera = camera;
        }

        public async Task Init(EcsSystems systems)
        {
            _ships = systems
                .GetWorld()
                .Filter()
                .With<TransformRef>().With<Player>().Build();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _ships)
            {
                var transform = entity.GetComponent<TransformRef>().Ref;

                var bounds = GetWorldScreenBounds();

                if (transform.position.x > bounds.x)
                {
                    transform.position = new Vector2(-bounds.x, transform.position.y);
                }

                if (transform.position.x < -bounds.x)
                {
                    transform.position = new Vector2(bounds.x, transform.position.y);
                }

                if (transform.position.y > bounds.y)
                {
                    transform.position = new Vector2(transform.position.x, -bounds.y);
                }

                if (transform.position.y < -bounds.y)
                {
                    transform.position = new Vector2(transform.position.x, bounds.y);
                }
            }
        }

        private Vector3 GetWorldScreenBounds()
        {
            var bounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));

            return bounds;
        }
    }
}