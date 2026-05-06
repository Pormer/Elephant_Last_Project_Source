using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI
{
    // 데이터 구독
    // 데이터 변경되면 이벤트 Invoke
    public abstract class ModelBase : MonoBehaviour
    {
        private Dictionary<Enum, object> _propertyStorage = new();

        public delegate void PropertyValueChanged(Enum key, object newValue);

        public event PropertyValueChanged OnPropertyValueChanged;
        protected bool _isInitialized = false;

        private void Awake()
        {
            Initialize();
            _isInitialized = true;
        }

        protected virtual void Initialize()
        {
        }

        public virtual async UniTask InvokeInitialValues()
        {
            if (!_isInitialized)
                await UniTask.WaitUntil(() => _isInitialized);
            foreach (var kvp in _propertyStorage)
            {
                OnPropertyValueChanged?.Invoke(kvp.Key, kvp.Value);
            }
        }

        public T GetProperty<T>(Enum propertyName)
        {
            if (_propertyStorage.TryGetValue(propertyName, out var value))
            {
                return (T)value;
            }

            return default;
        }

        protected void SetProperty<T>(Enum propertyName, T value, bool changeSame = false)
        {
            if (propertyName == null) return;
            if (_propertyStorage.TryGetValue(propertyName, out var currentValue))
            {
                if (!changeSame && EqualityComparer<T>.Default.Equals((T)currentValue, value))
                    return;
            }

            _propertyStorage[propertyName] = value;
            OnPropertyChanged(propertyName);
        }

        protected void NotifyProperty(Enum propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        protected void MutateProperty<T>(Enum propertyName, Action<T> mutator) where T : class
        {
            if (propertyName == null || mutator == null) return;
            if (_propertyStorage.TryGetValue(propertyName, out var currentValue) == false) return;
            if (currentValue is not T target) return;

            mutator(target);
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(Enum propertyName)
        {
            if (_propertyStorage.TryGetValue(propertyName, out var newValue))
            {
                OnPropertyValueChanged?.Invoke(propertyName, newValue);
            }
        }
    }
}
