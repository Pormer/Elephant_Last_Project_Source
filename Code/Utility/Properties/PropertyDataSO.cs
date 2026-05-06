using UnityEngine;

namespace Code.Utility.Properties
{
    public enum PropertyType
    {
        Int,
        Float,
        Color,
        Vector
    }
    
    [CreateAssetMenu(fileName = "Property data", menuName = "SO/Property/PropertyData", order = 0)]
    public class PropertyDataSO : ScriptableObject
    {
        public PropertyType propertyType;
        public string propertyName;
        public int propertyHash;

        private void OnValidate()
        {
            propertyHash = Shader.PropertyToID(propertyName);
        }
        
        private void OnEnable()
        {
            propertyHash = Shader.PropertyToID(propertyName);
        }
    }
}