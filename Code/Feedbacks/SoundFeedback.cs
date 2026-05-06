using Ami.BroAudio;
using UnityEngine;

namespace Feedbacks
{
    public class SoundFeedback : Feedback
    {
        [Header("Sound Data")]
        [SerializeField] private SoundID soundID;
        
        [Header("Play Point")]
        [SerializeField] private bool isUsePlayPoint;
        [SerializeField] private Transform soundPlayPoint;
        
        private IAudioPlayer _audioPlayer;
        
        public override void PlayFeedback()
        {
            _audioPlayer = isUsePlayPoint 
                ? BroAudio.Play(soundID, soundPlayPoint?.position ?? Vector3.zero) 
                : BroAudio.Play(soundID);
        }

        public override void StopFeedback()
        {
            if(_audioPlayer?.IsPlaying ?? false)
                _audioPlayer.Stop();
        }
    }
}