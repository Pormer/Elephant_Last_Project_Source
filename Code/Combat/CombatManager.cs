using System;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Combat
{
    [Provide]
    public class CombatManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private CombatInfoSO combatInfo;
        [SerializeField] private GameEventChannelSO combatChannel;

        private void Awake()
        {
            combatInfo.isCombat = false;
            
            combatChannel.AddListener<CombatStartEvent>(HandleCombatStart);
            combatChannel.AddListener<CombatEndEvent>(HandleCombatEnd);
        }

        private void OnDestroy()
        {
            combatChannel.RemoveListener<CombatStartEvent>(HandleCombatStart);
            combatChannel.RemoveListener<CombatEndEvent>(HandleCombatEnd);
        }

        private void HandleCombatStart(CombatStartEvent evt) => combatInfo.isCombat = true;

        private void HandleCombatEnd(CombatEndEvent evt) => combatInfo.isCombat = false;
    }
}