using System;
using Code.Contexts;
using Code.Contexts.Summons;
using Code.Modules;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.Summons.Base
{
    public abstract class Summon<T> : ModuleOwner, ISummon, IContextSetUp<T> where T : SummonContext
    {
        public ModuleOwner Owner { get; private set; } 
        public UnityEvent OnSummonEvent;
        public UnityEvent<Summon<T>> OnReleaseEvent;
        
        public virtual void SetUp(T context)
        {
            Owner = context.Owner;
            transform.position = context.Position;
            transform.eulerAngles = context.Rotation;
            
            OnSummonEvent?.Invoke();
        }

        public virtual void Release() // 풀에 푸시하거나 오브젝트 삭제
        {
            OnReleaseEvent?.Invoke(this);
        }
    }
}