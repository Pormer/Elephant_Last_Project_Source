using Code.Souls.Core;
using EventSystem;
using UnityEngine;

namespace Code.Tests
{
    public class SoulEquipTester : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO skillChannel;

        [Header("Test info")] 
        [SerializeField] private SoulDataSO targetAbilityData;
        [SerializeField] private SoulType slotType;

        [ContextMenu("Test Equip")]
        private void TestAbilityEquip()
        {
            var evt = SkillEvents.PlayerSoulEquipEvent.Initializer(slotType, targetAbilityData);
            skillChannel.RaiseEvent(evt);
        }
    }
}