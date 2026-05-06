using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Interface
{
    public interface ISelectableState
    {
        public Selectable DefaultFocusable { get; }
    }
}