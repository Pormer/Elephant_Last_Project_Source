using System;
using Ami.BroAudio;
using EventSystem;
using UnityEngine;

namespace Code.Feedbacks.Managers
{
    public class SoundFeedbackManager : FeedbackManager
    {
        [SerializeField] private SoundID defaultBGMId;
        private IAudioPlayer _bgmPlayer;
        
        private void Awake()
        {
            feedbackChannel.AddListener<BGMChangeEvent>(HandleBGMChange);
            feedbackChannel.AddListener<SetDefaultBGMEvent>(HandleSetDefaultBGM);
            feedbackChannel.AddListener<UseDefaultBGMEvent>(HandleUseDefaultBGM);
            feedbackChannel.AddListener<PauseBGMEvent>(HandlePauseBgm);
            
            BroAudio.OnBGMChanged += HandleBGMChanged;
        }

        private void Start()
        {
            PlayDefaultBGM();
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<BGMChangeEvent>(HandleBGMChange);
            feedbackChannel.RemoveListener<SetDefaultBGMEvent>(HandleSetDefaultBGM);
            feedbackChannel.RemoveListener<UseDefaultBGMEvent>(HandleUseDefaultBGM);
            feedbackChannel.RemoveListener<PauseBGMEvent>(HandlePauseBgm);
            
            BroAudio.OnBGMChanged -= HandleBGMChanged;
        }
        
        private void HandleBGMChanged(IAudioPlayer bgmPlayer) => _bgmPlayer = bgmPlayer;

        private void HandleBGMChange(BGMChangeEvent evt)
        {
            PlayBGM(evt.targetBgmID, 2f);
        }

        private void HandleSetDefaultBGM(SetDefaultBGMEvent evt)
        {
            defaultBGMId = evt.defaultBgmID;
            
            if(evt.isImmediate)
                PlayDefaultBGM();
        }
        
        private void HandleUseDefaultBGM(UseDefaultBGMEvent evt)
        {
            PlayDefaultBGM();
        }

        private void HandlePauseBgm(PauseBGMEvent evt)
        {
            if(evt.isStopBGM)
                _bgmPlayer?.Pause(1f);
            else
                _bgmPlayer?.UnPause(1f);
        }
        
        private void PlayDefaultBGM()
        {
            if(defaultBGMId.IsValid())
                PlayBGM(defaultBGMId, 2f);
            else
            {
                Debug.LogWarning($"BGM  ID {defaultBGMId} is invalid");
            }
        }

        private IAudioPlayer PlayBGM(SoundID bgmId, float fadeTime = 1f)
        {
            return bgmId.IsValid() 
                ? BroAudio.Play(bgmId).AsBGM().SetTransition(Transition.CrossFade, fadeTime) 
                : null;
        }
    }
}