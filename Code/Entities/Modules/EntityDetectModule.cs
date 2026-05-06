using System;
using System.Collections.Generic;
using Code.Modules;
using UnityEngine;

namespace Code.Entities.Modules
{
    public class EntityDetectModule : MonoBehaviour, IModule
    {
        public ContactFilter2D contactFilter;
        public float detectRange;
        public int detectCnt;
        
        private Entity _owner;
        private Collider2D[] _results;

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Entity;
            Debug.Assert(_owner != null, $"owner is not Entity : {owner.name}");
            
            _results = new Collider2D[detectCnt];
        }

        public bool TryDetectTarget<T>(out HashSet<T> results, Func<T, bool> condition = null) where T : MonoBehaviour
        {
            results = new HashSet<T>();
            
            int cnt = Physics2D.OverlapCircle(transform.position, detectRange, contactFilter, _results);
            if (cnt == 0) return false;

            for (int i = 0; i < cnt; ++i)
            {
                if (_results[i].TryGetComponent(out T item) && (condition == null || condition.Invoke(item)))
                    results.Add(item);
            }

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
}