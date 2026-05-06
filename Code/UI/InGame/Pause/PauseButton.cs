using System;
using Code.UI.InputBind;
using Code.UI.UIStateMachine;
using UnityEngine;

namespace Code.UI.InGame.Pause
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private PointerHandler pointerHandler;
        [SerializeField] private UIStateType uiStateType;

        public PointerHandler PointerHandler => pointerHandler;

        public Action<UIStateType> OnPressed;

        private void Awake()
        {
            pointerHandler.Pressed += HandlePressed;
        }

        private void OnDestroy()
        {
            pointerHandler.Pressed -= HandlePressed;
        }

        private void HandlePressed() => OnPressed?.Invoke(uiStateType);
    }
}