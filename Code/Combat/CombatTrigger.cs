using System;
using Code.Entities;
using Code.Extensions;
using Code.Stage;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class CombatTrigger : MonoBehaviour
    {
        public UnityEvent OnTriggerEnterEvent;
        
        [SerializeField] private EntityFinderSO playerFinder;
        [SerializeField] private GameEventChannelSO combatChannel;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private bool isLockCam = true;
        
        [SerializeField] private StageDataSO stageData;

        [Inject] private StageManager _stageManager;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsSameLayer(targetLayer))
            {
                OnTriggerEnterEvent?.Invoke();
                combatChannel.RaiseEvent(CombatEvents.CombatStartEvent);
                Destroy(gameObject);
                
                _stageManager.StartStage(stageData);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (stageData == null) return;

            Gizmos.color = Color.red;
            
            foreach (var spawnData in stageData.spawnDataList)
            {
                Gizmos.DrawCube(spawnData.spawnPos, Vector3.one);
            }
        }
#endif
    }
}