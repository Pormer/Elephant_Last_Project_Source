using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.UIStateMachine
{
    public enum UIStateType
    {
        None = -1,
        Title,
        Setting,
        Pause,
        Menu,
        MainHud,
        SoulEquip = 100,
        StatUpgrade,
        Map,
        TempEnd
    }

    public abstract class UIStateMachine<T, TEvt> : MonoBehaviour where T : IUIState where TEvt : UIChangeEvent
    {
        [SerializeField] protected UIStateType initialStateType = UIStateType.None;
        [SerializeField] protected GameEventChannelSO uiEventChannel;
        protected bool IsChangingState = false;
        protected Dictionary<UIStateType, T> StateDictionary = new Dictionary<UIStateType, T>();
        protected T PreviousState;
        public T CurrentState { get; protected set; }
        protected UIStateType CurrentStateType;


        protected virtual void Start()
        {
            uiEventChannel.AddListener<TEvt>(HandleUIStateChangeEvent);

            var states = GetComponentsInChildren<T>(true);
            StateDictionary.Clear();

            for (var i = 0; i < states.Length; i++)
            {
                var state = states[i];
                if (state == null) continue;

                if (StateDictionary.ContainsKey(state.StateType))
                {
                    Debug.LogWarning($"Duplicate UI state registration for {state.StateType}. Using first instance.",
                        this);
                    continue;
                }

                StateDictionary.Add(state.StateType, state);
            }
        }

        protected void InitializeStates()
        {
            foreach (var state in StateDictionary.Values)
            {
                if (state.StateType == initialStateType)
                {
                    state.GameObject.SetActive(true);
                    CurrentState = state;
                    CurrentStateType = state.StateType;
                    state.OnEnter().Forget();
                    PreviousState = state;
                    uiEventChannel.RaiseEvent(UIEvents.UIStateChangeEndedEvent.Initializer(state));
                }
                else
                {
                    state.OnExit().Forget();
                    state.GameObject.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            uiEventChannel.RemoveListener<TEvt>(HandleUIStateChangeEvent);
        }

        private async void HandleUIStateChangeEvent(TEvt obj)
        {
            if (IsChangingState) return;
            var newStateType = obj.NewStateType;
            if (obj.NewStateType == UIStateType.None && PreviousState != null)
                newStateType = PreviousState.StateType;

            if (!StateDictionary.TryGetValue(newStateType, out var newState)) return;

            IsChangingState = true;
            try
            {
                if (CurrentState != null && CurrentState.StateType != newStateType)
                {
                    HandleExitState(CurrentState);
                    await CurrentState.OnExit();
                }

                if (CurrentState == null || newStateType != CurrentState.StateType)
                {
                    newState.GameObject.SetActive(true);
                    PreviousState = CurrentState;
                    CurrentState = newState;
                    CurrentStateType = newStateType;
                    await newState.OnEnter();
                    HandleNewState(newState);
                    uiEventChannel.RaiseEvent(UIEvents.UIStateChangeEndedEvent.Initializer(newState));
                }

                if (PreviousState != null)
                {
                    await UniTask.NextFrame();
                    PreviousState.GameObject.SetActive(false);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e, this);
            }
            finally
            {
                IsChangingState = false;
            }
        }
        protected virtual void HandleExitState(T prevState)
        {
        }        
        protected virtual void HandleNewState(T newState)
        {
        }
    }
}