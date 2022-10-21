using UnityEngine;
using Random = UnityEngine.Random;

namespace _Client
{
    public class RotationAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 minSpeedByAxis;
        [SerializeField] private Vector3 maxSpeedByAxis;

        private Vector3 _speedByAxis;
        private Transform _cachedTransform;

        private void Start()
        {
            _cachedTransform = transform;
            _speedByAxis = new Vector3(
                Random.Range(minSpeedByAxis.x, maxSpeedByAxis.x), 
                Random.Range(minSpeedByAxis.y, maxSpeedByAxis.y), 
                Random.Range(minSpeedByAxis.z, maxSpeedByAxis.z));
        }

        private void Update()
        {
            _cachedTransform.Rotate(_speedByAxis.x * Time.deltaTime, _speedByAxis.y * Time.deltaTime, _speedByAxis.z * Time.deltaTime);
        }
    }
}