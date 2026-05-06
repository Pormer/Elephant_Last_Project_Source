using System;
using Code.Modules;
using UnityEngine;

namespace Code.Statuses
{
    public abstract class Status : MonoBehaviour
    {
        [field: SerializeField] public StatusDataSO StatusData { get; private set; }
        
        private float _timer;
        
        public event Action<StatusType> OnStatusEnd;

        public abstract void Initialize(ModuleOwner owner);

        public virtual void Apply(float duration)
        {
            _timer = duration;
        }

        public virtual void UpdateStatus()
        {
            DecreaseTimer();
        }

        public virtual void End()
        {
            _timer = 0f;
        }
        
        private void DecreaseTimer()
        {
            _timer -= Time.deltaTime;
                
            if (_timer <= 0f)
            {
                OnStatusEnd?.Invoke(StatusData.statusType);
            }
        }

        public virtual void Refresh(float duration)
        {
            Apply(duration);
            // 딩가딩가 중첩 기술
        }
    }
}