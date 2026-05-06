using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Collectibles.CollectableItems
{
    public abstract class CollectableItem : MonoBehaviour, ICollectable
    {
        public UnityEvent OnCollectCompleteEvent;
        public UnityEvent OnCollectStartEvent;

        [SerializeField] private float collectingSpeed = 3.0f;

        [SerializeField] private Vector3 collectTargetOffset;

        protected Coroutine _collectingCoroutine;
        protected Collider _collider;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();

            Debug.Assert(_collider, $"collider not found: {name}");
        }

        public virtual void StartCollecting(Transform collectorTrm)
        {
            if (_collectingCoroutine != null)
                StopCoroutine(_collectingCoroutine);

            _collider.enabled = false;

            OnCollectStartEvent?.Invoke();
            _collectingCoroutine = StartCoroutine(CollectingCoroutine(collectorTrm));
        }

        private IEnumerator CollectingCoroutine(Transform target)
        {
            yield return new WaitForSeconds(0.42f);
            
            Vector3 targetPos = target.TransformPoint(collectTargetOffset);
            float distance = Vector3.Distance(transform.position, targetPos);
            float completeTime = distance / collectingSpeed;
            float currentTime = 0;

            while (completeTime >= currentTime)
            {
                Vector3 startPos = transform.position;

                currentTime += Time.deltaTime;
                float t = currentTime / completeTime;

                transform.position = Vector3.Lerp(startPos, targetPos, t * t * t);

                targetPos = target.TransformPoint(collectTargetOffset);
                distance = Vector3.Distance(transform.position, targetPos);
                completeTime = distance / collectingSpeed;
                yield return null;
            }

            transform.position = targetPos;

            OnCollectCompleteEvent?.Invoke();
            CollectComplete();
        }

        public virtual void StopCollecting()
        {
            if (_collectingCoroutine != null)
                StopCoroutine(_collectingCoroutine);

            _collider.enabled = true;
        }

        public abstract void CollectComplete();
    }
}