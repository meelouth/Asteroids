using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Client
{
    public class LaserGunUIView : UIElement
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI value;

        public void SetFill(float amount)
        {
            slider.value = amount;
        }

        public void SetValue(float amount)
        {
            value.text = amount.ToString(CultureInfo.InvariantCulture);
        }
    }
}