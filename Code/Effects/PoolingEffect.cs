using System;
using System.Collections.Generic;
using System.Linq;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Effects
{
    public class PoolingEffect : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        [SerializeField] private List<GameObject> effectObjects;
        [SerializeField] private bool useDuration;
        [SerializeField] private float duration;
        [SerializeField] private bool followParent; //자식 playable위치, 회전 동기화가 필요할때(이펙트들 포지션, 회전 lock 해주기)

        private Pool _myPool;

        private HashSet<IPlayableVFX> _playableVFXs;

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _playableVFXs = effectObjects.Select(effect =>
            {
                IPlayableVFX playableVFX = effect.GetComponent<IPlayableVFX>();
                Debug.Assert(playableVFX != null, $"effect object must have IPlayableVFX component");
                return playableVFX;
            }).ToHashSet();
        }

        public void ResetItem()
        {
            foreach (IPlayableVFX playableVFX in _playableVFXs)
                playableVFX.StopVFX();
        }

        public async void PlayVFX(Vector3 position, Quaternion rotation)
        {
            if (followParent)
            {
                transform.position = position;
                transform.rotation = rotation;
            }

            foreach (IPlayableVFX playableVFX in _playableVFXs)
                playableVFX.PlayVFX(position, rotation);

            if (useDuration)
            {
                await Awaitable.WaitForSecondsAsync(duration);

                StopPoolEffect();
            }
        }

        public void SetDuration(float duration)
        {
            this.duration = duration;
        }

        public void StopPoolEffect()
        {
            try
            {
                foreach (IPlayableVFX playableVFX in _playableVFXs)
                    playableVFX.StopVFX();
                _myPool.Push(this);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void OnValidate()
        {
            if (effectObjects == null) return;

            foreach (GameObject effect in effectObjects.ToList())
            {
                IPlayableVFX playableVFX = effect.GetComponent<IPlayableVFX>();
                if (playableVFX == null)
                {
                    effectObjects.Remove(effect);
                    Debug.LogError($"effect object must have IPlayableVFX component");
                }
            }
        }
    }
}