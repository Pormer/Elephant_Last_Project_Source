using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI.InGame
{
    public class TempDeadHandler : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private int titleSceneIdx;
        public void OnPlayerDead()
        {
            uiChannel.RaiseEvent(UIEvents.UIFadeStartEvent.Initializer(true, ChangeScene));
        }
        private async void ChangeScene()
        {
            await UniTask.NextFrame();
            await SceneManager.LoadSceneAsync(titleSceneIdx);
        }
    }
}