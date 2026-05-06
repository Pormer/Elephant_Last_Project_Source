using System;
using System.Threading.Tasks;
using Code.Contexts.Summons;
using Code.ETC;
using Code.Modules;
using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Entities.Modules
{
    public class EntityRenderer : MonoBehaviour, IModule
    {
        [SerializeField] protected SpriteRenderer visual;
        [SerializeField] private bool isLookRight;
        
        private Tween _fadeTween;
        
        public bool IsFacingRight { get; private set; }
        public ModuleOwner Owner { get; private set; }
        public Material Material { get; private set; }

        public Color Color
        {
            get => visual.color;
            set => visual.color = value;
        } 

        public Sprite Sprite
        {
            get => visual.sprite;
            set => visual.sprite = value;
        }
        
        public void Initialize(ModuleOwner owner)
        {
            Owner = owner;
            Material = visual.material;
            IsFacingRight = isLookRight;
        }

        private void OnDestroy()
        {
            _fadeTween?.Kill();
        }

        public void SetFacingRight(bool isRight)
        {
            float currentYAngle = Owner.transform.eulerAngles.y;
            float targetYAngle = isRight == isLookRight ? 0 : 180; // 기본 스프라이트가 오른쪽을 보고 있냐에 따라 비교 값 계산

            if (Mathf.Approximately(currentYAngle, targetYAngle) == false)
            {
                Owner.transform.eulerAngles = new Vector3(0, targetYAngle, 0);
                IsFacingRight = isRight;
            }
        }

        public void CreateAfterImage(PoolItemSO item, AfterImageContext context, bool useCurrentSprite = false, bool useCurrentFlip = false)
        {
            if(useCurrentSprite)
                context.Sprite = visual.sprite;
            
            if(useCurrentFlip)
                context.IsFacingRight = isLookRight ? !IsFacingRight : IsFacingRight;
            
            AfterImage afterImage = PoolManagerMono.Instance.Pop<AfterImage>(item);
            afterImage.SetUp(context);
        }

        public void Fade(float endValue, float duration, Action onComplete = null)
        {
            _fadeTween?.Complete();
            _fadeTween = visual.DOFade(endValue, duration).OnComplete(() => onComplete?.Invoke());
        }
        
        public async Task StartDestroyEffect()
        {
            await Awaitable.WaitForSecondsAsync(0.4f);
            
            for (int i = 0; i < 2; i++)
            {
                visual.enabled = false;
                await Awaitable.WaitForSecondsAsync(0.13f);
                visual.enabled = true;
                await Awaitable.WaitForSecondsAsync(0.13f);
            }
            visual.enabled = false;
        }
    }
}