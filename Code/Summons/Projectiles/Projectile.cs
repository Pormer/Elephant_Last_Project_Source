using System;
using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Projectiles;
using Code.Skills.Core;
using Code.Summons.Base;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Summons.Projectiles
{
    public abstract class Projectile<T> : PoolingSummon<T>, IAttackable where T : ProjectileContext
    {
        public Transform AttackerTrm => transform;
        
        protected AttackDataSO _attackData;
        protected Rigidbody _rigid;
        
        protected Vector3 _dir;
        protected float _speed;
        protected int _damage;

        protected override void Awake()
        {
            base.Awake();
            
            _rigid = GetComponent<Rigidbody>();
        }
        
        public override void SetUp(T context)
        {
            base.SetUp(context);
            
            _dir = context.Dir;
            _speed = context.Speed;
            _damage = context.Damage;
            _attackData = context.AttackData;
        }
    }
}