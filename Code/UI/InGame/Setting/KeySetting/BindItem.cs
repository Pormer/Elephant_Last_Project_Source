using System;
using Code.UI.InputBind;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using Input;
using TMPro;
using UnityEngine;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame.Setting.KeySetting
{
    public abstract class BindItem : KeyIcon
    {
        [SerializeField] private UIViewElement listeningElement;
        [SerializeField] private string labelName;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private PointerHandler pointerHandler;
        
        public PointerHandler PointerHandler => pointerHandler;

        public Action<BindItem> OnBindItemClick;
        public Action<bool> OnBinding;
        
        protected UIInputBindSO BindSO { get; private set; }
        protected DeviceType DeviceType { get; private set; }

        private const string ListeningKey = "listen";
        private void Awake()
        {
            pointerHandler.Pressed += HandlePressed;

            label.text = labelName;
        }

        public virtual void Initialize(UIInputBindSO bind, DeviceType device)
        {
            if (BindSO != null)
            {
                BindSO.OnKeyChanged -= HandleKeyChanged;
            }

            BindSO = bind;
            DeviceType = device;

            if (BindSO != null)
            {
                BindSO.OnKeyChanged += HandleKeyChanged;
            }

            Refresh();
        }

        private void OnDestroy()
        {
            pointerHandler.Pressed -= HandlePressed;
            if (BindSO != null)
            {
                BindSO.OnKeyChanged -= HandleKeyChanged;
            }
        }

        private void HandlePressed()
        {
            OnBindItemClick?.Invoke(this);
            listeningElement.AddState(ListeningKey, 10).Forget();
            
            OnBinding?.Invoke(true);
            TriggerRebind();
        }

        private void HandleKeyChanged()
        {
            listeningElement.RemoveState(ListeningKey).Forget();
            OnBinding?.Invoke(false);
            Refresh();
        }

        protected abstract void Refresh();
        protected abstract void TriggerRebind();
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (label != null)
                label.text = labelName;
        }

#endif
    }
}
