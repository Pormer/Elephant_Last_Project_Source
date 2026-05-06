using UnityEngine;

namespace Code.Entities
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/Entity/EntityFinder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        public Entity Target { get; private set; }

        public void SetTarget(Entity target)
        {
            Target = target;
        }
    }
}