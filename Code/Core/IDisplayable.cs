using UnityEngine;

namespace Code.Core
{
    public interface IDisplayable
    {
        public Sprite Icon { get; }
        public string DisplayName { get; }
        public string Description { get; }
    }
}