using UnityEngine;

namespace _Client
{
    [CreateAssetMenu(menuName = "Configuration/Create New")]
    public class Configuration : ScriptableObject
    {
        [SerializeField] private ShipConfiguration shipConfiguration;
        [SerializeField] private AsteroidsConfiguration asteroidsConfiguration;
        [SerializeField] private PlayerScoringConfiguration playerScoringConfiguration;
        [SerializeField] private UFOConfiguration ufoConfiguration;
        
        public ShipConfiguration ShipConfiguration => shipConfiguration;
        public AsteroidsConfiguration AsteroidsConfiguration => asteroidsConfiguration;
        public PlayerScoringConfiguration PlayerScoringConfiguration => playerScoringConfiguration;
        public UFOConfiguration UFOConfiguration => ufoConfiguration;
    }
}