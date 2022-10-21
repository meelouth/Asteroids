using UnityEngine;

namespace _Client
{
    [System.Serializable]
    public class GunsConfiguration
    {
        [SerializeField] private GunConfiguration primaryGun;
        [SerializeField] private GunConfiguration secondaryGun;

        public GunConfiguration PrimaryGun => primaryGun;
        public GunConfiguration SecondaryGun => secondaryGun;
    }
}