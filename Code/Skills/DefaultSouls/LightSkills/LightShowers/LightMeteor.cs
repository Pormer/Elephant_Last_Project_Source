using System.Collections;
using Code.Combat.Attacks;
using Code.Contexts.Summons;
using Code.Detectors;
using Code.Effects;
using Code.Skills.Core;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Skills.DefaultSouls.LightSkills.LightShowers
{
    public class LightMeteor : SkillSummon<SkillSummonContext, LightShowerDataSO>, IPoolable, IAttackable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }

        [SerializeField] private PlayParticleVFX effect;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private float lifeTime;
        [SerializeField] private float damageCastTime;

        public GameObject GameObject => gameObject;
        public Transform AttackerTrm => transform;

        private WaitForSeconds _waitLifeTime;
        private WaitForSeconds _waitDamageCastTime;
        private LightShowerDataSO _data;
        private Pool _myPool;

        protected override void Awake()
        {
            base.Awake();
            
            _waitLifeTime = new WaitForSeconds(lifeTime - damageCastTime);
            _waitDamageCastTime = new WaitForSeconds(damageCastTime);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public override void SetUp(SkillSummonContext context)
        {
            base.SetUp(context);
            
            Init();
            effect.PlayVFX(transform.position, Quaternion.identity);
            StartCoroutine(ProgressCoroutine());
        }
        private void Init()
        {
            _data = _skill.SkillData as LightShowerDataSO;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator ProgressCoroutine()
        {
            yield return _waitDamageCastTime;
            
            damageCaster.StartCasting();
            damageCaster.CastDamage(_castedData.meteorCasterData, _damageContext, out _);
            
            yield return _waitLifeTime;
            Release();
        }

        public override void Release()
        {
            GoToPool();
        }

        private void GoToPool() => _myPool.Push(this);

        public void ResetItem()
        {
        }
    }
}