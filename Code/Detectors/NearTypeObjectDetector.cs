using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Detectors
{
    public class NearTypeObjectDetector : DetectController
    {
        public T GetClosestDamageable<T>(DetectorDataSO detectorData, Vector3 position)
        {
            var contexts = Detect(detectorData);
            
            T closestTarget = default;

            float closestDistance = Mathf.Infinity;

            foreach (var context in contexts)
            {
                if(context.Collider.TryGetComponent(out T target) == false) continue;

                float distance = Vector3.Distance(position, context.Collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }
    }
}