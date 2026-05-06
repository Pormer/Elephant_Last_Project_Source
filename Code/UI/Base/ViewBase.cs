using System;
using System.Collections.Generic;
using Code.UI.Interface;
using UnityEngine;

namespace Code.UI
{
    public class ViewBase : MonoBehaviour
    {
        protected readonly Dictionary<Enum, List<IUIBindable>> _bindableElements = new();

        public void Initialize()
        {
            _bindableElements.Clear();
            RegisterBindables(InitializeBindables());
        }

        protected virtual List<IUIBindable> InitializeBindables()
        {
            return new List<IUIBindable>();
        }

        protected void RegisterBindables(IEnumerable<IUIBindable> bindables)
        {
            if (bindables == null) return;

            foreach (var bindable in bindables)
            {
                if (bindable == null) continue;
                AddBindable(bindable);
            }
        }

        protected void AddBindable(IUIBindable bindable)
        {
            if (!_bindableElements.ContainsKey(bindable.BindKey))
                _bindableElements[bindable.BindKey] = new List<IUIBindable>();

            _bindableElements[bindable.BindKey].Add(bindable);
        }

        public void Bind(Enum key, object value)
        {
            if (_bindableElements.TryGetValue(key, out var bindables))
            {
                foreach (var bindable in bindables)
                {
                        bindable.Bind(value);
                }
            }

            HandleBinding(key, value);
        }

        protected virtual void HandleBinding(Enum key, object value) { }
    }
}
