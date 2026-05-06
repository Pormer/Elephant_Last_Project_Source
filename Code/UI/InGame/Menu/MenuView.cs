using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using Input;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu
{
    public class MenuView : SubUIStateMachine, ISelectableState
    {
        [SerializeField] private UIViewElement canvasElement;
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private GameEventChannelSO uiChannel;

        public Selectable DefaultFocusable => CurrentState is not ISelectableState focusableState
            ? null
            : focusableState.DefaultFocusable;

        public override UIStateType StateType => UIStateType.Menu;
        public override bool DoesStop => true;
        private const string HideKey = "hide";

        public override async UniTask OnEnter()
        {
            await base.OnEnter();
            canvas.enabled = true;
            // uiChannel.RaiseEvent(UIEvents.SubUIChangeEvent.Initializer(UIStateType.SkillEquip));

            await canvasElement.RemoveState(HideKey);
            uiChannel.AddListener<SubUICancelEvent>(HandleSubUICancel);
            uiInput.OnMenuPressed += HandleTabPressed;
        }

        public override async UniTask OnExit()
        {
            uiChannel.RemoveListener<SubUICancelEvent>(HandleSubUICancel);
            uiInput.OnMenuPressed -= HandleTabPressed;
            await base.OnExit();
            await canvasElement.AddState(HideKey);
            canvas.enabled = false;
        }

        private void HandleTabPressed()
        {
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.MainHud));
        }

        private void HandleSubUICancel(SubUICancelEvent obj) =>
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.MainHud));
    }
}