using System;
using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class BattleCamBundle : MonoBehaviour
    {
        public UnityEvent OnCurrentCombatStart;
        public UnityEvent OnCurrentCombatEnd;
        
        [SerializeField] private GameEventChannelSO combatChannel;
        [SerializeField] private CombatTrigger combatTrigger;

        private void Awake()
        {
            combatTrigger.OnTriggerEnterEvent.AddListener(HandleCombatStart);
        }

        private void HandleCombatStart()
        {
            combatChannel.AddListener<CombatEndEvent>(HandleCombatEnd);
            OnCurrentCombatStart?.Invoke();
        }

        private void HandleCombatEnd(CombatEndEvent evt)
        {
            OnCurrentCombatEnd?.Invoke();
            combatChannel.RemoveListener<CombatEndEvent>(HandleCombatEnd);
            combatTrigger.OnTriggerEnterEvent.RemoveListener(HandleCombatStart);
        }
    }
}