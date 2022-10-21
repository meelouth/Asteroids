using UnityEngine;

namespace _Client
{
    [CreateAssetMenu(menuName = "Configuration/Gun/Laser")]
    public class LaserGunConfiguration : GunConfiguration
    {
        [SerializeField] private float rechargeTime;
        [SerializeField] private int maxCharges;
        [SerializeField] private float distance;
        [SerializeField] private float width;
        
        [SerializeField] private string uiViewAssetKey;
        [SerializeField] private string rayViewAssetKey;

        public float RechargeTime => rechargeTime;

        public int MaxCharges => maxCharges;

        public float Distance => distance;

        public float Width => width;

        public string UIViewAssetKey => uiViewAssetKey;

        public string RayViewAssetKey => rayViewAssetKey;
    }
}