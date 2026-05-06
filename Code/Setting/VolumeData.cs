using System;
using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;

namespace Code.Setting
{
    public enum VolumeType
    {
        Master,
        BGM,
        SFX,
        UI,
        End
    }

    [Serializable]
    public class VolumeData : SaveData
    {
        public List<float> volumes = new((int)VolumeType.End);

        private BroAudioType[] _audioTypes =
            {
                BroAudioType.All, BroAudioType.Music, BroAudioType.SFX, BroAudioType.UI
            };
        
        public void SetVolume(VolumeType type, float value)
        {
            volumes[(int)type] = value;
            BroAudio.SetVolume(_audioTypes[(int)type], value);
        }

        public override void Load(string path)
        {
            base.Load(path);
            for (int i = 0; i < (int)VolumeType.End; i++)
                BroAudio.SetVolume(_audioTypes[i], volumes[i]);
        }
    }
}