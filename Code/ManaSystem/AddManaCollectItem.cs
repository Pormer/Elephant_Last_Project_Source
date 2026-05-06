using Code.Collectibles.CollectableItems;
using EventSystem;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.ManaSystem
{
    public class AddManaCollectItem : CollectableItem, IPoolable
    {
        [SerializeField] private int addManaAmount;
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private float defaultSize = 1f;
        [SerializeField] private float manaSizeOffset = 1f;
        
        private Rigidbody _rigidbody;
        private bool _canCollect;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody>();
            Debug.Assert(_rigidbody != null, "AddMana: rigidbody is not found");
        }

        public override void StartCollecting(Transform collectorTrm)
        {
            if (!_canCollect) return;

            _canCollect = false;
            base.StartCollecting(collectorTrm);
        }

        public override void CollectComplete()
        {
            var evt = PlayerEvents.PlayerAddManaEvent.Initializer(addManaAmount);
            playerChannel.RaiseEvent(evt);
            
            _myPool.Push(this);
        }

        public void AddForce(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            _canCollect = false;
            _rigidbody.useGravity = true;
            _collider.enabled = true;
            _collider.isTrigger = false;
            
            transform.localScale = 
                Vector3.one * (defaultSize + Random.Range(-manaSizeOffset, manaSizeOffset));
        }

        private void OnCollisionStay(Collision collision)
        {
            if (_canCollect) return;
            
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _canCollect = true;
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.useGravity = false;
                _collider.isTrigger = true;
            }
        }
    }
}