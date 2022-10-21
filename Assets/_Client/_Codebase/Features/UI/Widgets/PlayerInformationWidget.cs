using TMPro;
using UnityEngine;

namespace _Client
{
    public class PlayerInformationWidget : UIElement
    {
        [SerializeField] private TextMeshProUGUI coordinatesLabel;
        [SerializeField] private TextMeshProUGUI velocityLabel;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI rotationLabel;

        public PlayerInformationWidget DisplayVelocity(float velocity)
        {
            velocityLabel.text = $"Velocity: {velocity:#0.##}";

            return this;
        }

        public PlayerInformationWidget DisplayScore(int score)
        {
            scoreLabel.text = $"Score: {score}";

            return this;
        }

        public PlayerInformationWidget DisplayCoordinates(Vector2 position)
        {
            coordinatesLabel.text = $"Coordinates: [{position.x:#0.##}, {position.y:#0.##}]";

            return this;
        }

        public PlayerInformationWidget DisplayRotation(float angle)
        {
            rotationLabel.text = $"Angle: {angle:#0}°";

            return this;
        }
    }
}