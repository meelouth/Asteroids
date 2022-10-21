using UnityEngine;

namespace _Client
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private PlayerInformationWidget playerInformationWidget;
        [SerializeField] private SecondaryWeaponWidget secondaryWeaponWidget;
        [SerializeField] private LoseScreen loseScreen;
        [SerializeField] private LoadingScreen loadingScreen;

        public PlayerInformationWidget PlayerInformationWidget => playerInformationWidget;
        public SecondaryWeaponWidget SecondaryWeaponWidget => secondaryWeaponWidget;
        public LoseScreen LoseScreen => loseScreen;
        public LoadingScreen LoadingScreen => loadingScreen;
    }
}