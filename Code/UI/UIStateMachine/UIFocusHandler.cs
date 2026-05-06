using Code.UI.Interface;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Code.UI.UIStateMachine
{
    public class UIFocusHandler : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem;
        [SerializeField] private InputSystemUIInputModule inputModule;

        private Selectable _lastSelectedObject;
        private ISelectableState _selectableState;
        private bool _isFocusScheduled;

        public Selectable LastFocusedObject
        {
            get
            {
                if (!_lastSelectedObject)
                    _lastSelectedObject = null;

                return _lastSelectedObject;
            }
            set
            {
                if (!value)
                {
                    _lastSelectedObject = null;
                    return;
                }

                if (value == _lastSelectedObject) return;

                _lastSelectedObject = value;
                value.Select();
                uiChannel.RaiseEvent(UIEvents.FocusElementChange.Initializer(value));
            }
        }

        private bool _isFocusNeeded = false;

        private void Awake()
        {
            uiChannel.AddListener<UIStateChangeEndedEvent>(HandleUIStateChange);
            uiChannel.AddListener<SetUIInputEnableEvent>(HandleInputEnabled);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<UIStateChangeEndedEvent>(HandleUIStateChange);
            uiChannel.RemoveListener<SetUIInputEnableEvent>(HandleInputEnabled);
        }

        private void HandleInputEnabled(SetUIInputEnableEvent obj)
        {
            inputModule.enabled = obj.Enabled;
        }

        private void LateUpdate()
        {
            if (!_isFocusNeeded || _isFocusScheduled) return;
            _isFocusScheduled = true;
            FocusAtEndOfFrame().Forget();
        }

        private async UniTaskVoid FocusAtEndOfFrame()
        {
            try
            {
                await UniTask.WaitForEndOfFrame(this);
                if (!_isFocusNeeded || _selectableState == null || eventSystem == null) return;

                var currentSelectedGameObject = eventSystem.currentSelectedGameObject;
                var lastFocusedObject = LastFocusedObject;

                if (currentSelectedGameObject != null)
                {
                    if (!lastFocusedObject || currentSelectedGameObject != lastFocusedObject.gameObject)
                        LastFocusedObject = currentSelectedGameObject.GetComponent<Selectable>();
                }

                if (currentSelectedGameObject == null || !LastFocusedObject)
                    LastFocusedObject = _selectableState.DefaultFocusable;

                if (LastFocusedObject)
                    LastFocusedObject.Select();
            }
            finally
            {
                _isFocusScheduled = false;
            }
        }

        private void HandleUIStateChange(UIStateChangeEndedEvent obj)
        {
            if (obj.NewState is ISelectableState focusableState)
            {
                _isFocusNeeded = true;
                _selectableState = focusableState;

                LastFocusedObject = focusableState.DefaultFocusable;
            }
            else _isFocusNeeded = false;
        }
    }
}
