using System;
using System.Collections.Generic;
using System.Linq;
using Code.Souls.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class SoulListController : MonoBehaviour
    {
        [field: SerializeField] public SoulType SoulType { get; private set; }
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform root;
        [SerializeField] private Selectable emptyText;
        [SerializeField] private GameObject soulPrefab;
        [SerializeField] private GameObject stepPrefab;
        [SerializeField] private int maxElementsPerStep = 4;

        private List<Transform> _steps = new();
        private Dictionary<Transform, int> _stepDict = new();

        private List<SoulItem> _soulItems = new List<SoulItem>();
        private Dictionary<SoulDataSO, bool> _newSoulItems = new();

        public Action<SoulDataSO> OnItemFocused;

        public Selectable FirstSelectable
        {
            get
            {
                if (_soulItems != null && _soulItems.Count > 0) return _soulItems[0].PointerHandler;
                return emptyText;
            }
        }

        public void ToggleActive(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
        }

        public void CreateSoulList(List<SoulDataSO> soulList, ref List<Selectable> added, ref List<Selectable> removed)
        {
            if (soulList == null || soulList.Count < 0)
            {
                emptyText.gameObject.SetActive(true);
                return;
            }

            emptyText.gameObject.SetActive(false);
            var activeCount = 0;
            for (var i = 0; i < soulList.Count; i++)
            {
                var skillData = soulList[i];
                if (skillData == null) continue;
                _newSoulItems.TryAdd(skillData, true);

                if (activeCount >= _soulItems.Count)
                {
                    var newSoul = Instantiate(soulPrefab).GetComponent<SoulItem>();
                    _soulItems.Add(newSoul);
                    added.Add(newSoul.PointerHandler);
                    newSoul.Initialize();
                    newSoul.Focused += HandleHolderFocused;
                }

                var holder = _soulItems[activeCount];
                holder.SetInfo(skillData);
                holder.SetNew(_newSoulItems[skillData]);

                var step = GetStep();
                _stepDict[step]++;
                holder.transform.SetParent(step);

                activeCount++;
            }

            for (int i = _soulItems.Count - 1; i > activeCount - 1; i--)
            {
                var holder = _soulItems[i];
                holder.Focused -= HandleHolderFocused;
                removed.Add(holder.PointerHandler);
                Destroy(holder.gameObject);
                _soulItems.RemoveAt(i);
            }
        }

        private Transform GetStep()
        {
            if (_steps != null && _steps.Count > 0)
            {
                if (_stepDict.TryGetValue(_steps[^1], out var st) && st < maxElementsPerStep)
                    return _steps[^1];
            }

            var newStep = Instantiate(stepPrefab).transform;
            newStep.SetParent(root);
            _steps?.Add(newStep);
            _stepDict.Add(newStep, 0);
            return newStep;
        }

        private void HandleHolderFocused(SoulDataSO holder)
        {
            OnItemFocused?.Invoke(holder);
            _newSoulItems[holder] = false;
        }

        public void SetEquipped(SoulDataSO equippedSoul)
        {
            foreach (var soulItem in _soulItems)
            {
                soulItem.SetEquipped(soulItem.TargetData == equippedSoul);
            }
        }
    }
}