using System;
using EventSystem;
using Input;
using UnityEngine;

namespace Code.UI.UIStateMachine
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private PlayerInputSO playerInputSO;
        [SerializeField] private UIInputSO uiInputSO;

        private void Awake()
        {
            uiChannel.AddListener<InputChangeEvent>(HandleInputChange);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<InputChangeEvent>(HandleInputChange);
        }

        private void HandleInputChange(InputChangeEvent obj)
        {
            playerInputSO.SetEnabled(obj.PlayerEnabled);
            uiInputSO.SetEnabled(obj.UIEnabled);
        }
    }
}