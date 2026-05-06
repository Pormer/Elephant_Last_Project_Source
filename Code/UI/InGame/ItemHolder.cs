using System;
using Code.UI.InputBind;
using Code.UI.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public abstract class ItemHolder<T> : MonoBehaviour
    {
        [SerializeField] protected PointerHandler pointerHandler;
        [SerializeField] protected Image icon;
        public PointerHandler  PointerHandler => pointerHandler;

        public T TargetData { get; private set; }

        public Action<ItemHolder<T>> Defocused;
        public Action<ItemHolder<T>> Focused;
        public Action<ItemHolder<T>> Pressed;

        protected bool IsInitialized = false;

        public virtual void Initialize(T data)
        {
            TargetData = data;
            if (data != null)
                icon.sprite = GetIcon(data);
            else
                icon.sprite = null;
            InitializeEvent();
        }

        private void InitializeEvent()
        {
            if (IsInitialized) return;
            pointerHandler.Focused += HandleFocused;
            pointerHandler.Defocused += HandleDefocused;
            pointerHandler.Pressed += HandlePressed;
            IsInitialized = true;
        }

        private void OnDestroy()
        {
            ResetData();
        }

        protected void ResetData()
        {
            if (IsInitialized)
            {
                pointerHandler.Focused -= HandleFocused;
                pointerHandler.Defocused -= HandleDefocused;
                pointerHandler.Pressed -= HandlePressed;
            }
            TargetData = default(T);
            icon.sprite = null;
            IsInitialized = false;
        }

        protected abstract Sprite GetIcon(T data);
        public void SetInteractable(bool interactable) => pointerHandler.interactable = interactable;
        private void HandleFocused() => Focused?.Invoke(this);
        private void HandleDefocused() => Defocused?.Invoke(this);
        private void HandlePressed() => Pressed?.Invoke(this);
    }
}