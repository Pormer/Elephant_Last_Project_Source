using Code.Modules;
using UnityEngine;

namespace Code.Utility.Properties.Shaders
{
    public class ShaderPropertyModule : MonoBehaviour, IModule
    {
        [SerializeField] private ShaderPropertyManagerSO propertyManager;
        [SerializeField] private Renderer targetRenderer;
        
        private ModuleOwner _owner;
        private Material _targetMat;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _targetMat = targetRenderer.material;
        }

        public void SetValue<T>(PropertyDataSO propertyData, T newValue)
            => propertyManager.SetProperty(_targetMat, propertyData, newValue);
        
        public T GetValue<T>(PropertyDataSO propertyData)
            => propertyManager.GetPropertyValue<T>(_targetMat, propertyData);
        
        /*public void SetValueTo<T>(PropertyDataSO propertyData, T endValue, float duration)
            => DOTween.To(() => propertyManager.GetPropertyValue<T>(_targetMat, propertyData),
                value => propertyManager.SetProperty(_targetMat, propertyData, value),
                endValue, duration);*/
    }
}