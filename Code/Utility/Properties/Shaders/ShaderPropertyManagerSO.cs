using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Code.Utility.Properties.Shaders
{
    [CreateAssetMenu(fileName = "Shader property manager", menuName = "SO/Property/ShaderPropertyManager", order = 0)]
    public class ShaderPropertyManagerSO : ScriptableObject
    {
        [SerializeField] private List<PropertyDataSO> properties;

        private Dictionary<PropertyDataSO, MethodInfo> _setterMethodPairs;
        private Dictionary<PropertyDataSO, MethodInfo> _getterMethodPairs;

        private void OnEnable()
        {
            SetUp();
        }

        private void SetUp()
        {
            if(properties == null) return;
            
            _setterMethodPairs = new Dictionary<PropertyDataSO, MethodInfo>();
            _getterMethodPairs = new Dictionary<PropertyDataSO, MethodInfo>();
            foreach (PropertyDataSO property in properties)
            {
                (MethodInfo setter, MethodInfo getter) = FindMethod(property);
                _setterMethodPairs.Add(property, setter);
                _getterMethodPairs.Add(property, getter);
            }
        }

        private (MethodInfo, MethodInfo) FindMethod(PropertyDataSO property)
        {
            MethodInfo[] methods = typeof(Material).GetMethods(BindingFlags.Public | BindingFlags.Instance);

            bool HasParam(MethodInfo m)
            {
                ParameterInfo[] param = m.GetParameters();
                return param.Length >= 1 && param[0].ParameterType == typeof(int);
            }
            
            MethodInfo setter = methods.FirstOrDefault(m => m.Name == $"Set{property.propertyType}" && HasParam(m));
            MethodInfo getter = methods.FirstOrDefault(m => m.Name == $"Get{property.propertyType}" && HasParam(m));
            return (setter, getter);
        }

        public void SetProperty<T>(Material target, PropertyDataSO property, T newValue)
        {
            MethodInfo setter = _setterMethodPairs.GetValueOrDefault(property);
            Debug.Assert(setter != null, $"{property} does not have a setter or key");
            setter.Invoke(target, new object[] { property.propertyHash, newValue });
        }

        public T GetPropertyValue<T>(Material target, PropertyDataSO property)
        {
            MethodInfo getter = _getterMethodPairs.GetValueOrDefault(property);
            Debug.Assert(getter != null, $"{property} does not have a getter or key");
            return (T)getter.Invoke(target, new object[] { property.propertyHash });
        }
    }
}