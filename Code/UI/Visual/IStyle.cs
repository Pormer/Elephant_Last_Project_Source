using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Visual
{
    public interface IStyle
    {
        void Initialize(GameObject tar);
        UniTask AddState(string state, int priority = 0);
        UniTask RemoveState(string state);
        UniTask ClearStates();
    }
}