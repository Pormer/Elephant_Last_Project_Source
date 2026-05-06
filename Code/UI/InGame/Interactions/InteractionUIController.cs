using System.Collections.Generic;
using EventSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.UI.InGame.Interactions
{
    public class InteractionUIController : MonoBehaviour
    {
        [SerializeField] private PoolItemSO interactionItem;
        [SerializeField] private GameEventChannelSO uiChannel;
        [Inject] private PoolManagerMono _poolManager;

        private Dictionary<Transform, InteractionBox> _interactionBoxes = new Dictionary<Transform, InteractionBox>();

        private void Awake()
        {
            uiChannel.AddListener<InteractionShowEvent>(HandleInteractionShow);
            uiChannel.AddListener<InteractionHideEvent>(HandleInteractionHide);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<InteractionShowEvent>(HandleInteractionShow);
            uiChannel.RemoveListener<InteractionHideEvent>(HandleInteractionHide);
        }

        private void HandleInteractionShow(InteractionShowEvent obj)
        {
            Debug.Log("im a bvraibefsdf");
            if (!_interactionBoxes.TryGetValue(obj.Target, out InteractionBox box))
            {
                box = _poolManager.Pop<InteractionBox>(interactionItem);
            }

            box.transform.SetParent(transform);
            box.SetInteraction(obj.Text, obj.Target, obj.Offset);
            _interactionBoxes[obj.Target] = box;
        }

        private void HandleInteractionHide(InteractionHideEvent obj)
        {
            if (!_interactionBoxes.TryGetValue(obj.Target, out var box)) return;
            box.HideInteraction();
            _interactionBoxes.Remove(obj.Target);
        }
    }
}