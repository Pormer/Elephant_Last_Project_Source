using Ami.BroAudio;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Enemies.Core.Attack.AttackerData
{
    [CreateAssetMenu(fileName = "CastAttackerDataSO", menuName = "SO/Enemy/Attacker/CastAttackerDataSO", order = 0)]
    public class CastAttackerDataSO : AbstactAttackerDataSO
    {
        public DetectorDataSO detectorData;
        public bool isAlwaysUseImpulse;
        public SoundID soundId;
    }
}