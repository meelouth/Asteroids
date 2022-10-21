using UnityEngine;

namespace _Client
{
    public class LaserGun : IComponent
    {
        public float CooldownToRecharge;
        public float RemainingTimeToRecharge;
        public int MaxCharges;
        public int LeftCharges;
        public Transform Muzzle;
        public ParticleSystem Particle;
        public float Width;
        public float Distance;
    }
}