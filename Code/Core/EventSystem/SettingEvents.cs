using Code.Setting;

namespace EventSystem
{
    public class SettingEvents
    {
        public static LanguageChangeEvent OnLanguageChangeEvent = new LanguageChangeEvent();
        public static DeviceChangeEvent OnDeviceChangeEvent = new DeviceChangeEvent();
        // public static VolumeChangeEvent OnVolumeChangeEvent = new VolumeChangeEvent();
    }

    public class LanguageChangeEvent : GameEvent
    {
        public LanguageType Language;
        public LanguageChangeEvent Initializer(LanguageType language)
        {
            Language = language;
            return this;
        }
    }

    public class DeviceChangeEvent : GameEvent
    {
        public DeviceType DeviceType;

        public DeviceChangeEvent Initializer(DeviceType deviceType)
        {
            DeviceType  = deviceType;
            return this;
        }
    }

    // public class VolumeChangeEvent : GameEvent
    // {
    //     public VolumeType VolumeType;
    //     public float Value;
    //
    //     public VolumeChangeEvent Initializer(VolumeType volumeType, float value)
    //     {
    //         VolumeType  = volumeType;
    //         Value = value;
    //         return this;
    //     }
    // }
}