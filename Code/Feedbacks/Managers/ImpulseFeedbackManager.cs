using EventSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Feedbacks.Managers
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulseFeedbackManager : FeedbackManager
    {
        private CinemachineImpulseSource source;
        
        private void Awake()
        {
            source = GetComponent<CinemachineImpulseSource>();
            
            feedbackChannel.AddListener<ImpulseFeedbackEvent>(HandlePlayImpulse);
        }
        
        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<ImpulseFeedbackEvent>(HandlePlayImpulse);
        }
        
        private void HandlePlayImpulse(ImpulseFeedbackEvent evt)
        {
            var data = evt.impulseData;
            source.ImpulseDefinition.AmplitudeGain = data.amplitudeGain;
            source.ImpulseDefinition.FrequencyGain = data.frequencyGain;
            
            source.ImpulseDefinition.TimeEnvelope.AttackTime = data.attackTime;
            source.ImpulseDefinition.TimeEnvelope.AttackShape = data.attackShape;
            source.ImpulseDefinition.TimeEnvelope.SustainTime = data.sustain;
            source.ImpulseDefinition.TimeEnvelope.DecayTime = data.decayTime;
            source.ImpulseDefinition.TimeEnvelope.DecayShape = data.decayShape;
            
            source.GenerateImpulse(data.impulseForce);
        }

    }
}