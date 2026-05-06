using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.UIStateMachine
{
    public abstract class SubUIStateMachine : UIStateMachine<ISubUIState, SubUIChangeEvent>, IMainUIState
    {
        public GameObject GameObject => gameObject;
        public abstract UIStateType StateType { get; }
        public abstract bool DoesStop { get; }

        public virtual UniTask OnEnter()
        {
            InitializeStates();
            return UniTask.CompletedTask;
        }

        public virtual async UniTask OnExit()
        {
            foreach (var state in StateDictionary.Values)
            {
                state.OnExit().Forget();
            }
            await UniTask.CompletedTask;
        }
    }
}