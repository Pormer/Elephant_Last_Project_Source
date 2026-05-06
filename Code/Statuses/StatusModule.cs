using System;
using System.Collections.Generic;
using System.Linq;
using Code.Modules;
using UnityEngine;

namespace Code.Statuses
{
    public class StatusModule : MonoBehaviour, IModule, IManageStatus
    {
        [SerializeField] protected StatusDataListSO statusDataList;
        
        public event Action<StatusType> OnStatusApply; //상태 적용
        public event Action<StatusType> OnStatusRefresh; //상태 갱신(같은 상태 들어왔을 때)
        public event Action<StatusType> OnStatusEnd; //상태 종료
        
        public HashSet<StatusType> CurrentStatuses { get; protected set; } //현재 상태들
        public bool HasStatuses => CurrentStatuses.Count > 0; //상태가 있는지
        
        protected ModuleOwner _owner;
        
        private Dictionary<StatusType, Status> _statusDict;
        
        public virtual void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            CurrentStatuses = new HashSet<StatusType>();
            
            _statusDict = GetComponentsInChildren<Status>()
                .ToDictionary(status => status.StatusData.statusType, status => status);
            _statusDict.Values.ToList().ForEach(status =>
            {
                status.Initialize(owner);
                status.OnStatusEnd += HandleStatusEnd;
            });
        }

        private void HandleStatusEnd(StatusType type)
        {
            RemoveStatus(type);
        }

        protected virtual void Update()
        {
            foreach (var type in CurrentStatuses)
            {
                _statusDict[type].UpdateStatus();
            }
        }

        //현재 적용되고 있는 상태 타이머 감소

        
        public virtual bool ApplyStatus(StatusType statusType, float duration)
        {
            if (!_statusDict.TryGetValue(statusType, out Status status)) return false;
            
            StatusDataSO statusData = status.StatusData;
            
            //만약 같은 상태가 존재하면 갱신하기
            if (CurrentStatuses.Contains(statusType))
            {
                if (_statusDict.TryGetValue(statusType, out Status refreshStatus)) 
                    refreshStatus.Refresh(duration);
                
                OnStatusRefresh?.Invoke(statusType);
                return true;
            }
            
            //중첩 안되는 상태일 경우 우선순위 비교
            if (statusData.stackType == StatusStackType.NonStackable && GetNonStackableStatus(out StatusDataSO nonStackable))
            {
                if (nonStackable.priority > statusData.priority) return false;
                
                RemoveStatus(nonStackable.statusType); //새로운 상태의 우선순위가 더 높다면 기존 상태 제거
            }
            
            //상태 추가
            CurrentStatuses.Add(statusType);
            
            if (_statusDict.TryGetValue(statusType, out Status applyStatus)) 
                applyStatus.Apply(duration);
            
            OnStatusApply?.Invoke(statusType);
            return true;
        }

        public virtual void ClearStatus()
        {
            foreach (StatusType type in CurrentStatuses.ToList())
                RemoveStatus(type);
        }

        public virtual void RemoveStatus(StatusType type)
        {
            if (_statusDict.TryGetValue(type, out Status endStatus)) 
                endStatus.End();
            
            OnStatusEnd?.Invoke(type);
            CurrentStatuses.Remove(type);
        }

        //중첩가능한 상태가 있는지
        private bool GetNonStackableStatus(out StatusDataSO status)
        {
            status = null;

            foreach (StatusType type in CurrentStatuses)
            {
                StatusDataSO data = _statusDict[type].StatusData;
                if (data.stackType == StatusStackType.NonStackable)
                {
                    status = data;
                    return true;
                }
            }
            
            return false;
        }
        
        public virtual bool Has(StatusType type) => CurrentStatuses.Contains(type);
    }

    public struct StatusPair
    {
        public Status status;
        public StatusDataSO statusData;
    }
}