using System;
using System.Collections.Generic;
using System.Linq;
using Code.Entities.StatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.StatUpgrade
{
    public class StatUpgradeTreeRenderer
    {
        private readonly List<Transform> _levelRoots = new();
        private readonly Dictionary<StatUpgradeDataSO, UpgradeHolder> _upgradeHolders = new();
        private readonly List<UpgradeHolder> _createdHolders = new();

        public Selectable FirstObject
        {
            get
            {
                if(_createdHolders == null || _createdHolders.Count == 0) return null;
                return _createdHolders.FirstOrDefault()?.PointerHandler;
            }
        }

        public void RebuildTree(
            UpgradeHolder upgradeHolderPrefab,
            Transform listRoot,
            Transform fallbackRoot,
            float itemSpacing,
            float levelSpacing,
            float topMargin,
            List<List<StatUpgradeDataSO>> levels,
            Action<ItemHolder<StatUpgradeDataSO>> onFocused,
            Action<ItemHolder<StatUpgradeDataSO>> onPressed)
        {
            ClearTree(onFocused, onPressed);
            if (levels == null || levels.Count == 0 || upgradeHolderPrefab == null) return;

            var levelIndexByData = BuildLevelIndexMap(levels);
            var anchorLevel = GetAnchorLevelIndex(levels);
            var positions = BuildTreePositions(levels, levelIndexByData, anchorLevel, itemSpacing);

            for (int levelIndex = 0; levelIndex < levels.Count; levelIndex++)
            {
                var level = levels[levelIndex];
                if (level == null) continue;

                var levelRoot = ResolveLevelRoot(levelIndex, listRoot, fallbackRoot);
                ApplyLevelPosition(levelRoot, levelIndex, levelSpacing, topMargin);

                for (int i = 0; i < level.Count; i++)
                {
                    var data = level[i];
                    if (data == null || _upgradeHolders.ContainsKey(data)) continue;

                    var holder = UnityEngine.Object.Instantiate(upgradeHolderPrefab, levelRoot);
                    holder.Initialize(data);
                    holder.Focused += onFocused;
                    holder.Pressed += onPressed;
                    ApplyHolderPosition(holder.transform, positions, data);

                    _upgradeHolders[data] = holder;
                    _createdHolders.Add(holder);
                }
            }
        }

        public void UpdateLocked(List<StatUpgradeDataSO> lockedData)
        {
            var lockedSet = lockedData != null
                ? new HashSet<StatUpgradeDataSO>(lockedData)
                : null;

            foreach (var holder in _createdHolders)
            {
                if (holder == null) continue;
                holder.SetLock(lockedSet != null && lockedSet.Contains(holder.TargetData));
            }
        }

        public void UpgradeItem(StatUpgradeDataSO data)
        {
            if (data == null) return;
            if (_upgradeHolders.TryGetValue(data, out var holder))
                holder.Upgrade();
        }

        public void SetAllInteractable(bool interactable)
        {
            foreach (var holder in _createdHolders)
            {
                if (holder == null) continue;
                holder.SetInteractable(interactable);
            }
        }

        public void ClearTree(
            Action<ItemHolder<StatUpgradeDataSO>> onFocused,
            Action<ItemHolder<StatUpgradeDataSO>> onPressed)
        {
            foreach (var holder in _createdHolders)
            {
                if (holder == null) continue;
                holder.Focused -= onFocused;
                holder.Pressed -= onPressed;
                UnityEngine.Object.Destroy(holder.gameObject);
            }

            _createdHolders.Clear();
            _upgradeHolders.Clear();
        }

        private Transform ResolveLevelRoot(int levelIndex, Transform listRoot, Transform fallbackRoot)
        {
            if (levelIndex >= 0 && levelIndex < _levelRoots.Count && _levelRoots[levelIndex] != null)
                return _levelRoots[levelIndex];
            if (levelIndex >= _levelRoots.Count)
            {
                var go = new GameObject($"LevelRoot_{levelIndex}", typeof(RectTransform));
                var root = go.GetComponent<RectTransform>();
                root.SetParent(listRoot != null ? listRoot : fallbackRoot, false);
                root.anchorMin = new Vector2(0.5f, 1f);
                root.anchorMax = new Vector2(0.5f, 1f);
                root.pivot = new Vector2(0.5f, 0.5f);
                _levelRoots.Add(root);
                return root;
            }

            return fallbackRoot;
        }

        private Dictionary<StatUpgradeDataSO, int> BuildLevelIndexMap(List<List<StatUpgradeDataSO>> list)
        {
            var map = new Dictionary<StatUpgradeDataSO, int>();

            for (int i = 0; i < list.Count; i++)
            {
                var level = list[i];
                if (level == null) continue;

                for (int j = 0; j < level.Count; j++)
                {
                    var data = level[j];
                    if (data == null || map.ContainsKey(data)) continue;
                    map[data] = i;
                }
            }

            return map;
        }

        private int GetAnchorLevelIndex(List<List<StatUpgradeDataSO>> list)
        {
            var maxCount = -1;
            var maxIndex = 0;

            for (int i = 0; i < list.Count; i++)
            {
                var count = list[i]?.Count ?? 0;
                if (count > maxCount)
                {
                    maxCount = count;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        private Dictionary<StatUpgradeDataSO, float> BuildTreePositions(
            List<List<StatUpgradeDataSO>> levels,
            Dictionary<StatUpgradeDataSO, int> levelIndexByData,
            int anchorLevel,
            float itemSpacing)
        {
            var result = new Dictionary<StatUpgradeDataSO, float>();

            if (anchorLevel >= 0 && anchorLevel < levels.Count)
            {
                AssignEvenly(levels[anchorLevel], result, itemSpacing);
            }

            for (int level = anchorLevel - 1; level >= 0; level--)
            {
                AssignLevel(levels[level], result, data =>
                {
                    var dependents = GetDependentsInLevel(data, levels, levelIndexByData, level + 1);
                    return dependents.Select(d => result[d]).ToList();
                }, itemSpacing);
            }

            for (int level = anchorLevel + 1; level < levels.Count; level++)
            {
                AssignLevel(levels[level], result, data =>
                {
                    var refs = new List<float>();
                    var needs = data?.needStatUpgradeList;
                    if (needs == null) return refs;

                    for (int i = 0; i < needs.Count; i++)
                    {
                        var need = needs[i];
                        if (need == null || !result.TryGetValue(need, out var x)) continue;
                        refs.Add(x);
                    }

                    return refs;
                }, itemSpacing);
            }

            return result;
        }

        private void AssignEvenly(
            List<StatUpgradeDataSO> level,
            Dictionary<StatUpgradeDataSO, float> result,
            float itemSpacing)
        {
            if (level == null || level.Count == 0) return;

            var startX = -((level.Count - 1) * itemSpacing) * 0.5f;
            for (int i = 0; i < level.Count; i++)
            {
                var data = level[i];
                if (data == null) continue;
                result[data] = startX + (i * itemSpacing);
            }
        }

        private void AssignLevel(
            List<StatUpgradeDataSO> level,
            Dictionary<StatUpgradeDataSO, float> result,
            Func<StatUpgradeDataSO, List<float>> referenceSelector,
            float itemSpacing)
        {
            if (level == null || level.Count == 0) return;

            var raw = new List<(StatUpgradeDataSO data, float desired, int index)>(level.Count);
            for (int i = 0; i < level.Count; i++)
            {
                var data = level[i];
                if (data == null) continue;

                var refs = referenceSelector.Invoke(data);
                var desired = refs.Count > 0 ? refs.Average() : i * itemSpacing;
                raw.Add((data, desired, i));
            }

            if (raw.Count == 0) return;
            raw.Sort((a, b) =>
            {
                var cmp = a.desired.CompareTo(b.desired);
                return cmp != 0 ? cmp : a.index.CompareTo(b.index);
            });

            var resolved = new List<(StatUpgradeDataSO data, float desired, float x)>(raw.Count);
            for (int i = 0; i < raw.Count; i++)
            {
                var desired = raw[i].desired;
                if (i > 0)
                {
                    var minX = resolved[i - 1].x + itemSpacing;
                    if (desired < minX) desired = minX;
                }

                resolved.Add((raw[i].data, raw[i].desired, desired));
            }

            var desiredCenter = raw.Average(r => r.desired);
            var resolvedCenter = resolved.Average(r => r.x);
            var shift = desiredCenter - resolvedCenter;

            for (int i = 0; i < resolved.Count; i++)
            {
                var x = resolved[i].x + shift;
                result[resolved[i].data] = x;
            }
        }

        private List<StatUpgradeDataSO> GetDependentsInLevel(
            StatUpgradeDataSO data,
            List<List<StatUpgradeDataSO>> levels,
            Dictionary<StatUpgradeDataSO, int> levelIndexByData,
            int targetLevelIndex)
        {
            var dependents = new List<StatUpgradeDataSO>();
            if (data == null || targetLevelIndex < 0 || targetLevelIndex >= levels.Count) return dependents;

            var targetLevel = levels[targetLevelIndex];
            if (targetLevel == null) return dependents;

            for (int i = 0; i < targetLevel.Count; i++)
            {
                var candidate = targetLevel[i];
                if (candidate == null) continue;

                if (!levelIndexByData.TryGetValue(candidate, out var level) || level != targetLevelIndex)
                    continue;

                var needs = candidate.needStatUpgradeList;
                if (needs == null) continue;

                for (int j = 0; j < needs.Count; j++)
                {
                    if (needs[j] == data)
                    {
                        dependents.Add(candidate);
                        break;
                    }
                }
            }

            return dependents;
        }

        private void ApplyLevelPosition(
            Transform levelRootTransform,
            int levelIndex,
            float levelSpacing,
            float topMargin)
        {
            if (levelRootTransform == null) return;
            var y = -topMargin - (levelIndex * levelSpacing);

            if (levelRootTransform is RectTransform rectTransform)
            {
                var pos = rectTransform.anchoredPosition;
                pos.x = 0f;
                pos.y = y;
                rectTransform.anchoredPosition = pos;
            }
            else
            {
                var pos = levelRootTransform.localPosition;
                pos.y = y;
                levelRootTransform.localPosition = pos;
            }
        }

        private void ApplyHolderPosition(
            Transform holderTransform,
            Dictionary<StatUpgradeDataSO, float> positions,
            StatUpgradeDataSO data)
        {
            if (holderTransform == null || data == null) return;
            if (!positions.TryGetValue(data, out var x)) return;

            if (holderTransform is RectTransform rectTransform)
            {
                var pos = rectTransform.anchoredPosition;
                pos.x = x;
                rectTransform.anchoredPosition = pos;
            }
            else
            {
                var pos = holderTransform.localPosition;
                pos.x = x;
                holderTransform.localPosition = pos;
            }
        }
    }
}