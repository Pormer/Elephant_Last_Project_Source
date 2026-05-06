using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EasyTextEffects;
using GondrLib.ObjectPool.RunTime;
using TMPro;
using UnityEngine;

namespace Code.UI.InGame.Dialogue
{
    public class DialogueBox : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private UIViewElement element;
        [SerializeField] private ChoiceList choiceList;
        [SerializeField] private TextEffect textEffect;
        [SerializeField] private float autoDelay;
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _pool;
        private bool _auto;
        private string[] _choices;

        private Transform _target;
        private float _height;

        private const string HideKey = "hide";

        public Action<int, DialogueBox> OnChoice;

        private void OnDestroy()
        {
            choiceList.ChoicePressed -= HandleChoice;
            choiceList.SkipPressed -= HandleSkip;
        }

        private void HandleSkip()
        {
            textEffect.StopAllEffects();
            EffectEnded();
        }

        private void HandleChoice(int obj)
        {
            OnChoice?.Invoke(obj, this);
        }

        public async void EffectEnded()
        {
            if (_auto)
            {
                await UniTask.WaitForSeconds(autoDelay);
                OnChoice?.Invoke(0, this);
                return;
            }

            await UniTask.NextFrame();
            choiceList.SetChoiceList(_choices);
        }
        
        private void LateUpdate()
        {
            transform.position = _target.position + Vector3.up * _height;
        }

        public void SetDialogue(string dialogue, Transform target, float height, bool auto, string[] choices)
        {
            _target = target;
            _height = height;
            transform.position = target.position + Vector3.up * _height;
            dialogueText.text = dialogue;
            _auto = auto;
            _choices = choices;
            
            choiceList.ReadyForEnd();
            element.RemoveState(HideKey).Forget();
            textEffect.Refresh();
        }

        public async void EndDialogue()
        {
            choiceList.ChoicePressed -= HandleChoice;
            choiceList.SkipPressed -= HandleSkip;
            await element.AddState(HideKey);
            _pool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            element.AddState(HideKey).Forget();
            choiceList.ClearChoiceList();
            choiceList.ChoicePressed += HandleChoice;
            choiceList.SkipPressed += HandleSkip;
        }
    }
}
