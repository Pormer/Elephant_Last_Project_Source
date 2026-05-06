using Code.Contexts.Summons;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Summons.Base
{
    public abstract class PoolingSummon<T> : Summon<T>, IPoolable where T : SummonContext
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        
        public virtual void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public override void Release()
        {
            base.Release();
            
            _myPool.Push(this);
        }

        public virtual void ResetItem()
        {
        }
    }
}