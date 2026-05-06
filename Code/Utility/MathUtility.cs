using System;
using System.Collections.Generic;
using Code.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Utility
{
    public static class MathUtility
    {
        public static Vector3 UniformDonutPoint(float minRadius, float maxRadius)
        {
            Vector2 randPoint = Random.insideUnitCircle;
            Vector2 dir = randPoint.normalized;
            float radius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));
            return new Vector3(dir.x * radius, 0,  dir.y * radius);
        }

        public static T GetNearestTarget<T>(Vector3 standardPos, IEnumerable<GameObject> targets, bool isRemoveY = true, Func<T, bool> condition = null) where T : MonoBehaviour
        {
            T result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in targets)
            {
                if (target.TryGetComponent(out T castedTarget) == false || (condition != null && condition(castedTarget) == false)) continue;

                Vector3 targetPos = target.transform.position;
                float distance = isRemoveY ? 
                    Vector3.Distance(standardPos.RemoveY(), targetPos.RemoveY()) : 
                    Vector3.Distance(standardPos, targetPos);

                if (distance < minDistance)
                {
                    result = castedTarget;
                    minDistance = distance;
                }
            }

            return result;
        }
    }
}