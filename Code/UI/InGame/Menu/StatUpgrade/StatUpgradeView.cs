using System;
using System.Collections.Generic;
using Code.Entities.StatSystem;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.StatUpgrade
{
    public class StatUpgradeView : ViewBase, ISubUIState, ISelectableState
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement element;
        [Space(10)][Header("Upgrade Tree")]
        [SerializeField] private UpgradeDesc upgradeDesc;
        [SerializeField] private UpgradeHolder upgradeHolderPrefab;
        [SerializeField] private Transform listRoot;
        [SerializeField] private float itemSpacing = 180f;
        [SerializeField] private float levelSpacing = 180f;
        [SerializeField] private float topMargin = 40f;

        public GameObject GameObject => gameObject;
        
        private readonly StatUpgradeTreeRenderer _treeRenderer = new();
        private List<List<StatUpgradeDataSO>> _pendingTreeData;
        private bool _treeDirty;
        private bool _isEntered;

        public Selectable DefaultFocusable => _treeRenderer?.FirstObject; 

        private const string HideKey = "hide";

        public Action<StatUpgradeDataSO> UpgradeFocused;
        public Action<StatUpgradeDataSO> UpgradePressed;
        public Action CheckLocked;

        public UIStateType StateType => UIStateType.StatUpgrade;

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await element.RemoveState(HideKey);
            _isEntered = true;
            ApplyDeferredTreeIfNeeded();
        }

        public async UniTask OnExit()
        {
            _isEntered = false;
            await element.AddState(HideKey, 10);
            canvas.enabled = false;
        }

        protected override List<IUIBindable> InitializeBindables()
        {
            var res = new List<IUIBindable> { upgradeDesc };
            return res;
        }

        protected override void HandleBinding(Enum key, object value)
        {
            base.HandleBinding(key, value);
            switch (key)
            {
                case StatUIProperty.UpgradeDataList:
                    _pendingTreeData = value as List<List<StatUpgradeDataSO>>;
                    _treeDirty = true;
                    ApplyDeferredTreeIfNeeded();
                    break;
                case StatUIProperty.LockedDataList:
                    _treeRenderer.UpdateLocked(value as List<StatUpgradeDataSO>);
                    break;
            }
        }

        private void ApplyDeferredTreeIfNeeded()
        {
            if (!_isEntered || !_treeDirty) return;
            _treeDirty = false;
            _treeRenderer.RebuildTree(
                upgradeHolderPrefab,
                listRoot,
                transform,
                itemSpacing,
                levelSpacing,
                topMargin,
                _pendingTreeData,
                HandleHolderFocused,
                HandleHolderPressed);
        }

        public void UpgradeItem(StatUpgradeDataSO data) => _treeRenderer.UpgradeItem(data);

        public void SetAllInteractable(bool interactable) => _treeRenderer.SetAllInteractable(interactable);

        private void HandleHolderFocused(ItemHolder<StatUpgradeDataSO> holder) => UpgradeFocused?.Invoke(holder.TargetData);

        private void HandleHolderPressed(ItemHolder<StatUpgradeDataSO> holder) => UpgradePressed?.Invoke(holder.TargetData);

        private void OnDestroy()
        {
            _treeRenderer.ClearTree(HandleHolderFocused, HandleHolderPressed);
        }
    }
}
