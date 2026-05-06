using System;
using System.Collections;
using System.Collections.Generic;
using Code.Contexts.Summons;
using Code.Detectors;
using Code.Detectors.Datas;
using Code.Extensions;
using Code.Skills.Core;
using Code.Utility;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Skills.DefaultSouls.LightSkills.LightShowers
{
    public class LightShower : SkillSummon<SkillSummonContext, LightShowerDataSO>
    {
        public UnityEvent OnMeteorSpawnEvent;
        public UnityEvent OnFinalMeteorSpawnEvent;
        
        [SerializeField] private TargetDetector detector;
        [SerializeField] private DetectorDataSO targetDetectorData;

        private LightShowerDataSO _data;
        private ShaderPropertyModule _propertyModule;
        private Tween _emissionTween;
        private WaitForSeconds _waitTerm;
        private readonly Queue<GameObject> _targets = new Queue<GameObject>();
        private bool[] _targetingIdx;

        public override void SetUp(SkillSummonContext context)
        {
            base.SetUp(context);

            Init();
            ChangeDissolveTo(0, _data.ringDissolveDuration, () => StartCoroutine(ShowerCoroutine()));
        }

        private void Init()
        {
            _data = _skill.SkillData as LightShowerDataSO;
            _waitTerm = new WaitForSeconds(_data!.meteorTerm);
            transform.localScale = Vector3.one * _data.showerRange;
            _targetingIdx = new bool[_data.meteorCnt];

            _propertyModule = GetModule<ShaderPropertyModule>();
            _propertyModule.SetValue(_data.dissolveProperty, 1f);
        }

        private void OnDestroy()
        {
            _emissionTween?.Kill();
            StopAllCoroutines();
        }

        private IEnumerator ShowerCoroutine()
        {
            DetectAndSetTargeting();

            for (int i = 0; i < _data.meteorCnt; ++i)
            {
                yield return _waitTerm;
                Vector3 targetPos = GetTargetPos(_targetingIdx[i]);
                PopMeteor(_data.meteorItem, targetPos);
                OnMeteorSpawnEvent?.Invoke();
            }

            yield return new WaitForSeconds(_data.finalMeteorTerm);
            PopMeteor(_data.finalMeteorItem, transform.position);
            OnFinalMeteorSpawnEvent?.Invoke();

            yield return new WaitForSeconds(_data.ringDissolveStartDelay);
            ChangeDissolveTo(1, _data.ringDissolveDuration, Release);
        }

        private void DetectAndSetTargeting()
        {
            detector.CheckTargetDetected(targetDetectorData, out HashSet<GameObject> detectedTargets);
            
            foreach (GameObject target in detectedTargets)
                _targets.Enqueue(target);

            for (int i = 0; i < detectedTargets.Count; ++i)
            {
                if (i > _data.meteorCnt) break;

                _targetingIdx[i] = true;
            }

            _targetingIdx.Shuffle(100);
        }

        private void PopMeteor(PoolItemSO item, Vector3 targetPos)
        {
            LightMeteor meteor = PoolManagerMono.Instance.Pop<LightMeteor>(item);
            _damageContext.Attacker = meteor;
            SkillSummonContext context = new SkillSummonContext(this, targetPos, Vector3.zero, _skill, _damageContext);
            meteor.SetUp(context);
        }

        private void ChangeDissolveTo(float endValue, float duration, Action onComplete = null)
        {
            _emissionTween = DOTween.To(() => _propertyModule.GetValue<float>(_data.dissolveProperty),
                value => _propertyModule.SetValue(_data.dissolveProperty, value),
                endValue, duration).OnComplete(() => onComplete?.Invoke());
        }

        private Vector3 GetTargetPos(bool isTargeting)
        {
            if (isTargeting)
            {
                Vector3 targetPos = _targets.Dequeue().transform.position;
                targetPos.y = transform.position.y;

                if (Vector3.Distance(targetPos, transform.position) < _data.detectRange)
                    return targetPos;
            }
            
            return transform.position + MathUtility.UniformDonutPoint(_data.meteorSpawnMinRadius, _data.meteorSpawnMaxRadius);
        }

        public override void Release()
        {
            base.Release();
            Destroy(gameObject);
        }
    }
}