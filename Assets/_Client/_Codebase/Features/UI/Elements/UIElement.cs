using UnityEngine;

namespace _Client
{
    public class UIElement : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.gameObject.SetActive(false);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}