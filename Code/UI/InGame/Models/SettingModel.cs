using System.Collections.Generic;
using System.Linq;
using Code.Setting;
using UnityEngine;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame.Models
{
    public enum SettingUIProperty
    {
        Language,
        Device,
        VolumeValues,
        DisplayDatas,
    }

    public class SettingModel : ModelBase
    {
        [SerializeField] private UserSettingSO settingSo;

        #region Properties

        public LanguageType Language
        {
            get => GetProperty<LanguageType>(SettingUIProperty.Language);
            set => SetProperty(SettingUIProperty.Language, value);
        }

        public DeviceType Device
        {
            get => GetProperty<DeviceType>(SettingUIProperty.Device);
            set => SetProperty(SettingUIProperty.Device, value);
        }

        public Dictionary<VolumeType, float> VolumeValues
        {
            get => GetProperty<Dictionary<VolumeType, float>>(SettingUIProperty.VolumeValues);
            set => SetProperty(SettingUIProperty.VolumeValues, value);
        }

        public DisplayData DisplayDatas
        {
            get => GetProperty<DisplayData>(SettingUIProperty.DisplayDatas);
            set => SetProperty(SettingUIProperty.DisplayDatas, value);
        }

        #endregion

        protected override void Initialize()
        {
            base.Initialize();
            Language = settingSo.userData.language;
            var vv = new Dictionary<VolumeType, float>();
            for (var i = 0; i < settingSo.volumeData.volumes.Count; i++)
                vv[(VolumeType)i] = settingSo.volumeData.volumes[i];

            VolumeValues = vv;

            DisplayDatas = settingSo.displayData;
            DisplaySizeChanger.ChangeDisplaySize(DisplayDatas.IsFullScreen, DisplayDatas.Resolution);
        }

        public void SetLanguage(LanguageType language)
        {
            Language = language;
            settingSo.SetLanguage(language);
        }

        public void SetDevice(DeviceType device)
        {
            Device = device;
            settingSo.SetDeviceType(device);
        }

        public void SetVolume(VolumeType volumeType, float volume)
        {
            settingSo.SetVolume(volumeType, volume);
            VolumeValues[volumeType] = volume;
        }

        public void SetDisplay(bool fullscreen, ResolutionType resolution)
        {
            settingSo.SetDisplay(fullscreen, resolution);
        }
    }
}