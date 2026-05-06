using System;
using System.Collections.Generic;
using System.Linq;
using EventSystem;
using UnityEngine;

namespace Code.Explorations.Core
{
    public class ExplorationManager : MonoBehaviour
    {
        [SerializeField] private AreaDataListSO areaDataList;
        [SerializeField] private GameEventChannelSO stageChannel;

        private Dictionary<AreaDataSO, bool> _areaUnlockPairs;

        private void Awake()
        {
            _areaUnlockPairs = areaDataList.dataList.ToDictionary(data => data, _ => false);
            stageChannel.AddListener<AreaUnlockRequestEvent>(HandleAreaUnlock);
        }

        private void OnDestroy()
        {
            stageChannel.RemoveListener<AreaUnlockRequestEvent>(HandleAreaUnlock);
        }

        private void HandleAreaUnlock(AreaUnlockRequestEvent evt)
        {
            AreaDataSO targetArea = evt.targetArea;
            
            if (IsUnlocked(targetArea) || _areaUnlockPairs.ContainsKey(targetArea) == false) return;
            
            print($"Unlocked!: {targetArea}");
            _areaUnlockPairs[targetArea] = true;
            stageChannel.RaiseEvent(StageEvents.AreaUnlockEvent.Initializer(targetArea));
        }

        public bool IsUnlocked(AreaDataSO areaData) => _areaUnlockPairs.GetValueOrDefault(areaData);

        [Header("Test stage")]
        [SerializeField] private AreaDataSO testArea;
        
        [ContextMenu("Print active state")]
        private void PrintActiveState()
        {
            print($"{testArea} : {(IsUnlocked(testArea)? "잠금 해제 됨" : "잠겨있음")}");
        }
    }
}