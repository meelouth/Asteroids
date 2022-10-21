using System;
using UnityEngine;

namespace _Client
{
    public class SecondaryWeaponWidget : UIElement
    {
        [SerializeField] private RectTransform root;

        public void AttachToRoot(Transform view)
        {
            view.SetParent(root, false);
        }
    }
}