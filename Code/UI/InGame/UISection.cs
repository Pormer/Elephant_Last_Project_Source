using System.Collections.Generic;
using System.Linq;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public abstract class UISection : MonoBehaviour
    {
        [SerializeField] private UIViewElement uiViewElement;
        public abstract Selectable FirstSelectable { get; }
        protected List<Selectable> Selectables = new List<Selectable>();

        private const string FocusKey = "focus";

        private void Awake()
        {
            Selectables = GetComponentsInChildren<Selectable>().ToList();
            Initialize();
        }
        
        protected virtual void Initialize(){}

        protected void AddSelectable(Selectable selectable) => Selectables.Add(selectable);
        protected void RemoveSelectable(Selectable selectable) => Selectables.Remove(selectable);
        public virtual void InitializeView(){}

        public virtual async UniTask SetFocus(bool value)
        {
            foreach (var selectable in Selectables)
            {
                selectable.interactable = value;
            }
            if (value)
            {
                FirstSelectable?.Select();
                await uiViewElement.AddState(FocusKey);
            }
            else
            {
                await uiViewElement.RemoveState(FocusKey);
            }
        }
    }
}