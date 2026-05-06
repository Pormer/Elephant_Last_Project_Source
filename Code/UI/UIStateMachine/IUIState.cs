using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.UIStateMachine
{
    public interface IUIState
    {
        UIStateType StateType { get; }
        GameObject GameObject { get; }
        UniTask OnEnter();
        UniTask OnExit();
    }
}