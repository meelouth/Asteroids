using UnityEngine;

namespace _Client
{
    public abstract class GunConfiguration : ScriptableObject
    {
        [SerializeField] private GunType type;

        public GunType Type => type;
    }
}