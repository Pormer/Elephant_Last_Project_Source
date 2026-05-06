using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.InputList
{
    [RequireComponent(typeof(InputMapListView))]
    public class InputMapUpdater : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        private bool _needsLayoutRebuild;
        
        private InputMapListView _inputMapListView;
        
        private void Awake()
        {
            _inputMapListView = GetComponent<InputMapListView>();
            uiChannel.AddListener<SelectedElementChange>(HandleSelectElementChange);
            _needsLayoutRebuild = true;
        }

        private void OnEnable()
        {
            if (_needsLayoutRebuild)
            {
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.MarkLayoutForRebuild(_inputMapListView.RectTransform);
                _needsLayoutRebuild = false;
            }
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<SelectedElementChange>(HandleSelectElementChange);
        }
        
        private void HandleSelectElementChange(SelectedElementChange obj)
        {
            bool changed;
            if (obj.SelectedObject != null && obj.SelectedObject.TryGetComponent<InputMap>(out var mappable))
                changed = _inputMapListView.SetInfos(mappable);
            else
                changed = _inputMapListView.SetInfos(null);

            if (!changed) return;

            if (gameObject.activeInHierarchy)
            {
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.MarkLayoutForRebuild(_inputMapListView.RectTransform);
                _needsLayoutRebuild = false;
            }
            else
            {
                _needsLayoutRebuild = true;
            }
        }
    }
}