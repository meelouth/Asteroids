using UnityEngine;

namespace _Client
{
    public class ColliderView : MonoBehaviour
    {
        private EcsWorld _world;
        private Entity _entity;

        public void Init(Entity entity, EcsWorld world)
        {
            _entity = entity;
            _world = world;
        }

        public Entity GetEntity()
        {
            return _entity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ColliderView>(out var otherCollider))
            {
                var entity = _world.CreateEntity();
                var collision = entity.GetOrAddComponent<Collision>();

                collision.From = this;
                collision.Other = otherCollider;
            }
        }
    }
}