using Code.Collectibles.CollectableItems;
using Code.Souls.Core;
using DG.Tweening;
using EventSystem;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Code.Souls.SoulPieces
{
    public class SoulPiece : SelfCollectableItem
    {
        [Header("Data")]
        [SerializeField] private GameEventChannelSO skillChannel;
        [SerializeField] private SoulDataSO targetSoulData;

        [Header("Visual")]
        [SerializeField] private SpriteLibrary visualLibrary;
        [SerializeField] private Transform visualTrm;
        [SerializeField] private float selectScaleSize = 1.2f;

        public override void CollectComplete()
        {
            var evt = SkillEvents.SoulActiveEvent.Initializer(targetSoulData, true);
            skillChannel.RaiseEvent(evt); 
            
            _collider.enabled = true;
            SetActive(gameObject, false);
        }

        public override void Select(bool isSelect)
        {
            float scale = isSelect ? selectScaleSize : 1f;
            
            visualTrm.DOKill();
            visualTrm.DOScale(Vector3.one * scale, 0.2f).SetEase(Ease.OutSine);
        }

        private void SetActive(GameObject obj, bool isActive) => obj.SetActive(isActive);

        private void OnValidate()
        {
            if (targetSoulData != null && visualLibrary != null)
            {
                visualLibrary.spriteLibraryAsset = targetSoulData.auraData.petSpriteLibraryData;
            }
            else
            {
                visualLibrary.spriteLibraryAsset = null;
            }
        }
    }
}