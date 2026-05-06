using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using GondrLib.ObjectPool.RunTime;
using TMPro;
using UnityEngine;

namespace Code.UI.InGame.Interactions
{
    public class InteractionBox : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshProUGUI interactionText;
        [SerializeField] private UIViewElement element;
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }

        private Transform _target;
        private float _offset;

        private bool _readyToHide;
        
        private const string HideKey = "hide";
        
        public GameObject GameObject => gameObject;
        private Pool _pool;
        public void SetUpPool(Pool pool) => _pool = pool;

        public void ResetItem()
        {
            element.AddState(HideKey).Forget();
        }

        private void LateUpdate()
        {
            transform.position = _target.position + Vector3.up * _offset;
        }

        public void SetInteraction(string dialogue, Transform target, float offset)
        {
            _readyToHide = false;
            _target = target;
            _offset = offset;
            transform.position = target.position + Vector3.up * _offset;
            interactionText.text = dialogue;
            element.RemoveState(HideKey).Forget();
        }
        public async void HideInteraction()
        {
            _readyToHide = true;
            await element.AddState(HideKey);
            if(_readyToHide)
                _pool.Push(this);
        }
    }
}