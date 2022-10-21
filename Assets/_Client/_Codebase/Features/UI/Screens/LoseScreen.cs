using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Client
{
    public class LoseScreen : UIScreen
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private Button restartButton;

        private EcsWorld _world;

        private void OnEnable()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        public void Init(EcsWorld world)
        {
            _world = world;
        }
        
        public void SetScoreCount(int count)
        {
            scoreLabel.text = $"Score: {count}";
        }

        private void OnRestartButtonClicked()
        {
            var entity = _world.CreateEntity();
            entity.AddComponent<RestartGameCommand>();
        }
    }
}