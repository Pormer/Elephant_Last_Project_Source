using Code.Setting;
using Code.UI.InGame.Models;

namespace Code.UI.InGame.Setting
{
    public class SettingPresenter : PresenterBase<SettingModel, SettingView>
    {
        private DeviceChangeDetector _deviceChangeDetector;

        public override void Initialize(ModelBase m)
        {
            base.Initialize(m);
            _deviceChangeDetector = new DeviceChangeDetector();
            _deviceChangeDetector.OnDeviceChanged += HandleDeviceChange;
            _deviceChangeDetector.Initialize();

            view.OnVolumeChanged += HandleVolumeChanged;
            view.OnRevertScreen += HandleRevertScreen;
            view.OnRevertDisplay += HandleRevertDisplay;
            view.OnApplyDisplay += HandleApplyDisplay;
            view.OnLanguageChange += HandleLanguageChange;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_deviceChangeDetector != null)
            {
                _deviceChangeDetector.OnDeviceChanged -= HandleDeviceChange;
                _deviceChangeDetector.Dispose();
                _deviceChangeDetector = null;
            }

            view.OnVolumeChanged -= HandleVolumeChanged;
            view.OnRevertScreen -= HandleRevertScreen;
            view.OnRevertDisplay -= HandleRevertDisplay;
            view.OnApplyDisplay -= HandleApplyDisplay;
            view.OnLanguageChange -= HandleLanguageChange;
        }

        private void HandleLanguageChange(LanguageType obj)
        {
            model.SetLanguage(obj);
        }

        private void HandleRevertScreen()
        {
            DisplaySizeChanger.ChangeDisplaySize(model.DisplayDatas.IsFullScreen, model.DisplayDatas.Resolution);
            view.SetDisplaySetting(model.DisplayDatas.IsFullScreen, model.DisplayDatas.Resolution);
        }

        private void HandleRevertDisplay()
        {
            view.SetDisplaySetting(model.DisplayDatas.IsFullScreen, model.DisplayDatas.Resolution);
        }

        private void HandleApplyDisplay(bool fullScreen, ResolutionType resolution)
        {
            model.SetDisplay(fullScreen, resolution);
        }

        private void HandleVolumeChanged(VolumeType volume, float value)
        {
            model.SetVolume(volume, value);
        }

        private void HandleDeviceChange(DeviceType deviceType)
        {
            if (model.Device == deviceType) return;
            model.SetDevice(deviceType);
        }
    }
}