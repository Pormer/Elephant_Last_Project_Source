using Code.Skills;
using Code.Souls.Core;
using EventSystem;
using UnityEngine;

namespace Code.Tests
{
    public class ActiveSoulTester : MonoBehaviour
    {
        [SerializeField] private SoulDataListSO soulDataListData;
        [SerializeField] private GameEventChannelSO skillChannel;
        
        [ContextMenu("Active Skill")]
        private void TestActiveSkills()
        {
            foreach (var data in soulDataListData.values)
            {
                var evt = SkillEvents.SoulActiveEvent.Initializer(data, true);
                skillChannel.RaiseEvent(evt);
            }
        }
    }
}