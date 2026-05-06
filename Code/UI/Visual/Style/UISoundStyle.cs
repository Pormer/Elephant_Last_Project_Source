using System;
using System.Collections.Generic;
using System.Linq;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class UISoundStyle : MonoBehaviour, IStyle
    {
        [SerializeField] private List<SoundStyleData> styleDatas = new List<SoundStyleData>();
        private Dictionary<string, SoundID> _states;
        private IAudioPlayer _audioPlayer;

        public void Initialize(GameObject tar)
        {
            _states = styleDatas.ToDictionary(x => x.State, x => x.SoundID);
        }

        public UniTask AddState(string state, int priority = 0)
        {
            if (_states.TryGetValue(state, out SoundID soundID))
            {
                if(_audioPlayer != null && _audioPlayer.IsPlaying) _audioPlayer.Stop();
                _audioPlayer = BroAudio.Play(soundID);
            }

            return UniTask.CompletedTask;
        }

        public UniTask RemoveState(string state)=>UniTask.CompletedTask;

        public UniTask ClearStates()=> UniTask.CompletedTask;
    }

    [Serializable]
    public class SoundStyleData
    {
        public string State = "default";
        public SoundID SoundID;
    }
}