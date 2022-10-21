using UnityEngine;

namespace _Client
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPosition;
        [SerializeField] private Camera camera;

        public Transform PlayerSpawnPosition => playerSpawnPosition;
        public Camera Camera => camera;
    }
}