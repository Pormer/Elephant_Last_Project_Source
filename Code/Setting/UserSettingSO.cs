using System.IO;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Code.Setting
{
    [CreateAssetMenu(fileName = "PlayerSettingSO", menuName = "SO/Setting/PlayerSettingSO")]
    public class UserSettingSO : ScriptableObject
    {
        [SerializeField] private GameEventChannelSO settingChannel;
        [SerializeField] private string folderName;
        public UserData userData;
        public VolumeData volumeData;
        public DisplayData displayData;

        public string Path => System.IO.Path.Combine(Application.persistentDataPath, folderName);
        public const string JsonKey = ".json";
        public string GetPath(string fileName) => System.IO.Path.Combine(Path, $"{fileName}{JsonKey}");

        public void Initialize()
        {
            LoadSaveFile();
            settingChannel.RaiseEvent(SettingEvents.OnDeviceChangeEvent.Initializer(userData.deviceType));
            settingChannel.RaiseEvent(SettingEvents.OnLanguageChangeEvent.Initializer(userData.language));
        }

        private void LoadSaveFile()
        {
            userData.Load(GetPath(userData.FileName));
            volumeData.Load(GetPath(volumeData.FileName));
            displayData.Load(GetPath(displayData.FileName));
        }

        public void Dispose()
        {
        }


        public async void SetLanguage(LanguageType language)
        {
            userData.SetLanguage(language);
            userData.Save(GetPath(userData.FileName));

            await LocalizationSettings.InitializationOperation.Task;
            
            var locale = LocalizationSettings.AvailableLocales.GetLocale(language.ToString());
            if(locale != null)
                LocalizationSettings.SelectedLocale = locale;
            else
                Debug.LogWarning($"[PlayerSettingSo] Language {language} not supported");

            settingChannel.RaiseEvent(SettingEvents.OnLanguageChangeEvent.Initializer(userData.language));
        }

        public void SetDeviceType(DeviceType dtype)
        {
            userData.SetDeviceType(dtype);
            settingChannel.RaiseEvent(SettingEvents.OnDeviceChangeEvent.Initializer(userData.deviceType));
        }

        public void SetVolume(VolumeType volumeType, float volume)
        {
            volumeData.SetVolume(volumeType, volume);
            volumeData.Save(GetPath(volumeData.FileName));
        }

        public void SetDisplay(bool fullscreen, ResolutionType resolutionType)
        {
            displayData.SetFullScreen(fullscreen);
            displayData.SetResolution(resolutionType);
            displayData.Save(GetPath(displayData.FileName));
        }

#if UNITY_EDITOR
        [ContextMenu("UpdateDevice")]
        public void UpdateDevice()
        {
            settingChannel.RaiseEvent(SettingEvents.OnDeviceChangeEvent.Initializer(userData.deviceType));
        }

        [ContextMenu("SaveFile")]
        public void SaveFile()
        {
            userData.Save(GetPath(userData.FileName));
            volumeData.Save(GetPath(volumeData.FileName));
        }

        [ContextMenu("Reset Jsons")]
        public void ResetJsons()
        {
            //Just destroy directory :D
            if (File.Exists(Path))
                File.Delete(Path);
            else
                Debug.Log($"File {Path} does not exist");
        }
#endif
    }
}