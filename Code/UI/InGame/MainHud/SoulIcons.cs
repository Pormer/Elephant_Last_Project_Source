using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Skills;
using Code.Souls.Core;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.MainHud
{
    public class SoulIcons : MonoBehaviour, IUIBindable
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private Image[] soulIcon;
        [SerializeField] private UIViewElement[] elements;
        [SerializeField] private GameObject subIcon;
        [SerializeField] private Sprite playerIcon;

        private Dictionary<SoulType, SoulDataSO> SoulDatas;

        private bool _subIconActive;
        private SoulType _activeSoulType;

        private const string EffectKey = "effect";

        public Enum BindKey => SoulUIProperty.CurrentSoulSlot;

        private void Awake()
        {
            uiChannel.AddListener<PlayerSoulChangeEvent>(HandleSoulSwap);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<PlayerSoulChangeEvent>(HandleSoulSwap);
        }

        private async void HandleSoulSwap(PlayerSoulChangeEvent obj)
        {
            _activeSoulType = obj.targetSoul.soulType;
            if (!_subIconActive) return;
            if (SoulDatas == null) return;
            Sprite inactiveSoul;
            Sprite activeSoul;

            if (obj.targetSoul.soulType == SoulType.God)
            {
                inactiveSoul = SoulDatas[SoulType.Devil].Icon;
                activeSoul = SoulDatas[SoulType.God].Icon;
            }
            else
            {
                inactiveSoul = SoulDatas[SoulType.God].Icon;
                activeSoul = SoulDatas[SoulType.Devil].Icon;
            }

            UniTask tasks;
            tasks = AnimateElement(0, activeSoul);
            tasks = UniTask.WhenAll(tasks, AnimateElement(1, inactiveSoul));
            await tasks;
        }

        private async UniTask AnimateElement(int idx, Sprite sprite)
        {
            await elements[idx].AddState(EffectKey);
            soulIcon[idx].sprite = sprite;
            await elements[idx].RemoveState(EffectKey);
        }

        public void Bind(object v)
        {
            if (v is not Dictionary<SoulType, SoulDataSO> value || value.Count == 0)
            {
                SetEmpty();
                return;
            }

            SoulDatas = value;

            if (value.Count > elements.Length)
            {
                Debug.Log($"[SoulIcons] {value.Count} skills are not supported]");
                return;
            }

            bool doesGodExist = value.TryGetValue(SoulType.God, out var god);
            bool doesDevilExist = value.TryGetValue(SoulType.Devil, out var devil);

            if (!doesGodExist && !doesDevilExist)
            {
                SetEmpty();
                return;
            }

            if (doesGodExist != doesDevilExist)
            {
                _activeSoulType = doesGodExist ? SoulType.God : SoulType.Devil;
                SetSubIconActive(false);
            }
            else
                SetSubIconActive(true);

            if (_activeSoulType == SoulType.God)
            {
                soulIcon[0].sprite = doesGodExist ? value[SoulType.God].Icon : null;
                soulIcon[1].sprite = doesDevilExist ? value[SoulType.Devil].Icon : null;
            }
            else
            {
                soulIcon[0].sprite = doesDevilExist ? value[SoulType.Devil].Icon : null;
                soulIcon[1].sprite = doesGodExist ? value[SoulType.God].Icon : null;
            }
        }

        private void SetSubIconActive(bool v)
        {
            subIcon.SetActive(v);
            _subIconActive = v;
        }

        private void SetEmpty()
        {
            SetSubIconActive(false);
            soulIcon[0].sprite = playerIcon;
        }
    }
}