using UnityEngine;

namespace Code.Core
{
    public abstract class DisplayableSO : ScriptableObject, IDisplayable
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}