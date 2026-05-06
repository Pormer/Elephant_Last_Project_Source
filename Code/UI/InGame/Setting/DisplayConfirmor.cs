using System.Threading;
using Code.Setting;
using Cysharp.Threading.Tasks;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.InGame.Setting
{
    public class DisplayConfirmor : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI revertTimer;
        [SerializeField] private PointerHandler applyButton;
        [SerializeField] private PointerHandler revertButton;
        [SerializeField] private UIInputSO inputSo;
        [SerializeField] private int delayUntilCancel;

        private string _timerFormat;
        private int _timer;
        private bool _isTimerGoing;

        private int _choice = -1;
        private CancellationToken _timerToken;

        private void Awake()
        {
            _timerFormat = revertTimer.text;
        }

        public async UniTask<bool> ApplySetting(bool fullScreen, ResolutionType resolution)
        {
            _choice = -1;
            DisplaySizeChanger.ChangeDisplaySize(fullScreen, resolution);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            applyButton.Pressed += HandleApply;
            revertButton.Pressed += HandleRevert;

            _timer = delayUntilCancel;
            applyButton.Select();
            Timer();

            inputSo.OnCancelPressed += HandleRevert;
            var choose = UniTask.WaitUntil(() => _choice > -1);
            var time = UniTask.WaitUntil(() => _timer <= 0);
            
            await UniTask.WhenAny(choose, time);
            inputSo.OnCancelPressed -= HandleRevert;
            _timer = 0;

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            
            return _choice == 1;
        }

        private void HandleApply()
        {
            _choice = 1;
        }

        private void HandleRevert()
        {
            _choice = 0;
        }

        private async void Timer()
        {
            if (_isTimerGoing) return;
            _isTimerGoing = true;
            
            while (_timer > 0)
            {
                revertTimer.text = string.Format(_timerFormat, _timer);
                await UniTask.WaitForSeconds(1, true);
                _timer--;
            }
            
            _isTimerGoing = false;
        }
    }
}