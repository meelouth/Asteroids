using UnityEngine;

namespace _Client
{
    public class AsteroidView : MonoBehaviour
    {
        [SerializeField] private Transform model;
        
        public void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }

        public void SetModelRotation(Quaternion rotation)
        {
            model.rotation = rotation;
        }
    }
}