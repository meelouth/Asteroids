using UnityEngine;

namespace _Client
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Configuration/Gun/BulletGun")]
    public class BulletGunConfiguration : GunConfiguration
    {
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletLifetime;

        public float BulletSpeed => bulletSpeed;
        public float BulletLifetime => bulletLifetime;
    }
}