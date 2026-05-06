using System;
using UnityEngine;

namespace Code.Setting
{
    public enum LanguageType
    {
        ko,
        en
    }
    public enum DeviceType
    {
        None=-1,
        Keyboard,
        Xbox,
        PS
    }
    [Serializable]
    public class UserData : SaveData
    {
        public LanguageType language;
        public DeviceType deviceType;
        
        public void SetLanguage(LanguageType lan) => language = lan;
        public void SetDeviceType(DeviceType dtype) => deviceType = dtype;
    }
}