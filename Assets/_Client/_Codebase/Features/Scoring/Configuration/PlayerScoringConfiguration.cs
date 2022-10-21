using UnityEngine;

namespace _Client
{
    [System.Serializable]
    public class PlayerScoringConfiguration
    {
        [SerializeField] private int scoreByDestroyingObjective;

        public int ScoreByDestroyingObjective => scoreByDestroyingObjective;
    }
}