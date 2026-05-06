using System;
using System.Collections.Generic;
using Code.UI.InputBind;
using UnityEngine;

namespace Code.UI.InGame.InputList
{
    [Serializable]
    public struct InputInfo
    {
        public UIInputType UiInputType;
        public string Name;
    }

    [Serializable]
    public struct AdditionalInputInfos
    {
        public string key;
        public List<InputInfo> Infos;
    }

    public class InputMap : MonoBehaviour
    {
        [field: SerializeField] public List<InputInfo> Infos { get; private set; }
        [field: SerializeField] public List<AdditionalInputInfos> AdditionalInfos { get; private set; }

        private Dictionary<string, AdditionalInputInfos> _additionalInputInfosMap;
        private Dictionary<UIInputType, string> _defaultInfoDict;
        private Dictionary<UIInputType, string> _infoDict;

        private string _currentKey;

        public bool InfoDictExist => _infoDict != null || (Infos != null && Infos.Count > 0);

        public Dictionary<UIInputType, string> GetInfoDict()
        {
            if (_infoDict == null)
                InitializeInfoDict();

            return _infoDict;
        }

        private void InitializeInfoDict()
        {
            _defaultInfoDict = new Dictionary<UIInputType, string>();
            if (Infos != null)
            {
                foreach (var info in Infos)
                {
                    _defaultInfoDict[info.UiInputType] = info.Name;
                }
            }

            _infoDict = _defaultInfoDict;

            if (AdditionalInfos != null)
            {
                _additionalInputInfosMap = new Dictionary<string, AdditionalInputInfos>();
                foreach (var info in AdditionalInfos)
                {
                    _additionalInputInfosMap[info.key] = info;
                }
            }
        }

        public void SetKey(string key = "")
        {
            if(_infoDict == null) InitializeInfoDict();
            _currentKey = key;
            _infoDict = _defaultInfoDict;
            if (_additionalInputInfosMap.TryGetValue(key, out var additionalInfo))
            {
                foreach (var info in additionalInfo.Infos)
                {
                    _infoDict[info.UiInputType] = info.Name;
                }
            }
        }
    }
}