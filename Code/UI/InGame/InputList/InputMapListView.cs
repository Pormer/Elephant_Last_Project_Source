using System;
using System.Collections.Generic;
using Code.UI.InputBind;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.InputList
{
    [RequireComponent(typeof(LayoutGroup))]
    public class InputMapListView : MonoBehaviour
    {
        private static readonly UIInputType[] InputTypes = (UIInputType[])Enum.GetValues(typeof(UIInputType));

        [SerializeField] private Transform listRoot;
        [SerializeField] private GameObject inputMapPrefab;
        [SerializeField] private UIInputBindSO uiInputBind;
        [SerializeField] private GameEventChannelSO uiChannel;

        public RectTransform RectTransform { get; private set; }

        private Dictionary<UIInputType, InputMapItem> _infos = new();

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            uiInputBind.OnKeyChanged += InitializeBind;
            InitializeBind();
            SetInfos(null);
        }

        private void OnDestroy()
        {
            uiInputBind.OnKeyChanged -= InitializeBind;
        }

        private void InitializeBind()
        {
            for (var i = 0; i < InputTypes.Length; i++)
            {
                var type = InputTypes[i];
                if (!_infos.TryGetValue(type, out var info))
                {
                    info = Instantiate(inputMapPrefab, listRoot).GetComponent<InputMapItem>();
                    _infos.Add(type, info);
                }

                info.SetBindKey(uiInputBind.GetUIKey(type));
                if (info.gameObject.activeSelf)
                    info.gameObject.SetActive(false);
            }
        }

        public bool SetInfos(InputMap inputInfoList)
        {
            var changed = false;
            if (inputInfoList == null || !inputInfoList.InfoDictExist)
            {
                foreach (var info in _infos)
                {
                    if (info.Value.gameObject.activeSelf)
                    {
                        info.Value.gameObject.SetActive(false);
                        changed = true;
                    }
                }

                return changed;
            }

            var infos = inputInfoList.GetInfoDict();
            for (var i = 0; i < InputTypes.Length; i++)
            {
                var type = InputTypes[i];
                var target = _infos[type];
                if (infos.TryGetValue(type, out var text) && !string.IsNullOrEmpty(text))
                {
                    if (!target.gameObject.activeSelf)
                    {
                        target.gameObject.SetActive(true);
                        changed = true;
                    }
                    changed |= target.SetText(text);
                }
                else
                {
                    if (target.gameObject.activeSelf)
                    {
                        target.gameObject.SetActive(false);
                        changed = true;
                    }
                }
            }

            return changed;
        }
    }
}
