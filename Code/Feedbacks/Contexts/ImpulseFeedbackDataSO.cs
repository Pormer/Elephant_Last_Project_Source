using UnityEngine;

namespace Feedbacks.Contexts
{
    [CreateAssetMenu(fileName = "Impulse_FeedbackData", menuName = "SO/Feedbacks/ImpulseData", order = 0)]
    public class ImpulseFeedbackDataSO : ScriptableObject
    {
        [Header("Default Value")] 
        [Tooltip("추가되는 힘")] public float impulseForce = 1;
        [Tooltip("진동의 크기")] public float amplitudeGain;
        [Tooltip("진동의 속도")] public float frequencyGain;
        
        [Header("Time Envelope")]
        [Tooltip("최대치에 도달하는 시간")] public float attackTime;
        [Tooltip("최대치까지의 커브")] public AnimationCurve attackShape;
        [Tooltip("최대 진동이 유지되는 시간")] public float sustain;
        [Tooltip("진동이 서서히 멈추는 시간")] public float decayTime;
        [Tooltip("진동이 서서히 멈추는 커브")] public AnimationCurve decayShape;
    }
}