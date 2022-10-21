using UnityEngine;

namespace _Client
{
    public class PoolObject : MonoBehaviour
    {
        public Transform CachedTransform { get; private set; }

        private PoolContainer _container;

        private void Awake()
        {
            CachedTransform = transform;
        }

        public void SetContainer(PoolContainer container)
        {
            _container = container;
        }

        public void Return()
        {
            if (_container != null)
                _container.Return(this);
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}