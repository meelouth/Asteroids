using UnityEngine;

namespace _Client
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private Transform muzzleRoot;

        public Transform Muzzle => muzzleRoot;
    }
}