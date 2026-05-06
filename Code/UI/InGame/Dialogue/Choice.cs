using System;
using Code.UI.Visual;
using TMPro;
using UnityEngine;

namespace Code.UI.InGame.Dialogue
{
    public class Choice : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private PointerHandler pointer;
        
        public PointerHandler Pointer => pointer;
        
        private int _index;
        public Action<int> OnPressed;

        public void Initialize()
        {
            pointer.Pressed += HandlePressed;
        }

        private void OnDestroy()
        {
            pointer.Pressed -= HandlePressed;
        }

        private void HandlePressed() => OnPressed?.Invoke(_index);

        public void SetText(string text, int idx)
        {
            label.text = $"{idx + 1}. {text}";
            _index = idx;
        }
    }
}