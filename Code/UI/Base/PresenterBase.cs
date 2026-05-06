using System;
using UnityEngine;

namespace Code.UI
{
    public abstract class PresenterBase : MonoBehaviour
    {
        public abstract Type ModelType { get; }
        public abstract void Initialize(ModelBase m);
    }

    public abstract class PresenterBase<TModel, TView> : PresenterBase where TModel : ModelBase where TView : ViewBase
    {
        [SerializeField] protected TView view;
        protected TModel model;
        public override Type ModelType => typeof(TModel);

        private bool _isInitialized = false;
        private bool _isInitializing = false;

        public override async void Initialize(ModelBase m)
        {
            if (_isInitialized || _isInitializing)
                return;
            _isInitializing = true;

            model = m as TModel;
            if (!view)
                view = GetComponent<TView>();
            if (view == null || model == null)
            {
                _isInitializing = false;
                return;
            }

            view.Initialize();

            model.OnPropertyValueChanged += OnModelPropertyChanged;

            await model.InvokeInitialValues();
            _isInitializing  = false;
            _isInitialized = true;
        }

        protected virtual void OnModelPropertyChanged(Enum propertyName, object newValue)
        {
            view.Bind(propertyName, newValue);
        }

        protected virtual void OnDestroy()
        {
            if (model != null)
                model.OnPropertyValueChanged -= OnModelPropertyChanged;
        }
    }
}
