using System;
using System.Collections.Generic;
using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Combats;
using Code.Detectors;
using Code.Detectors.Datas;
using Code.Entities.Modules;
using Code.Entities.StatSystem;
using UnityEngine;

namespace Code.Players.AttackSystem
{
    public class AttackComboHandler : MonoBehaviour
    {
        [field: SerializeField] public AttackComboDataSO AttackComboData { get; private set; } //인덱스가 콤보 순서
        [SerializeField] private DamageCaster damageCaster;

        private Player _owner;
        private EntityRenderer _entityRenderer;
        private DamageModule _damageModule;

        public void Initialize(Player player)
        {
            _owner = player;
            _entityRenderer = _owner.GetModule<EntityRenderer>();
            _damageModule = _owner.GetModule<DamageModule>();

            Debug.Assert(_entityRenderer != null, "entity renderer is not found");
            Debug.Assert(_damageModule != null, "damage module is not found");
        }

        public bool ComboAttack(int comboIdx, StatSO damageStat, out AttackActionDataSO actionData,
            out HashSet<IDamageable> hits)
        {
            if (comboIdx < 0 || comboIdx >= AttackComboData.MaxComboCount)
            {
                Debug.LogWarning($"index: {comboIdx} is out of range");
                actionData = null;
                hits = null;
                return false;
            }

            actionData = AttackComboData[comboIdx];
            var damageContext = CreateDamageContext(damageStat, actionData.attackData);

            damageCaster.StartCasting();
            return damageCaster.CastDamage(actionData.casterData, damageContext, out hits);
        }

        public AttackDataSO GetCurrentAttackComboData(int comboIdx)
        {
            if (comboIdx < 0 || comboIdx >= AttackComboData.MaxComboCount)
            {
                Debug.LogWarning($"index: {comboIdx} is out of range");
                return null;
            }

            var comboData = AttackComboData[comboIdx];
            return comboData.attackData;
        }

        private DamageContext CreateDamageContext(StatSO damageStat, AttackDataSO attackData)
        {
            float lookAtXDir = _entityRenderer.IsFacingRight ? 1 : -1;
            
            var context = _damageModule.CalculateDamage(
                damageStat, attackData,
                _owner, _owner,
                out bool isCritical, true, lookAtXDir
            );

            return context;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (UnityEditor.EditorApplication.isPlaying) return;
            if (AttackComboData == null || AttackComboData.MaxComboCount <= 0) return;

            Gizmos.color = Color.yellow;
            Vector3 originPos = transform.position;

            foreach (var data in AttackComboData.actions)
            {
                if (data == null) continue;

                Vector3 position = originPos + data.hitPoint;
                Gizmos.DrawSphere(position, 0.1f);
            }

            Gizmos.color = Color.red;
            foreach (var data in AttackComboData.actions)
            {
                if (data == null || data.casterData == null || data.attackData == null) continue;

                Vector3 position = originPos + data.casterData.localPosition;
                switch (data.casterData.detectorShapeType)
                {
                    case DetectorShapeType.OverlapSphere:
                        Gizmos.DrawWireSphere(position, data.casterData.radius);
                        break;
                    case DetectorShapeType.OverlapBox:
                        Gizmos.DrawWireCube(position, data.casterData.boxSize);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Gizmos.color = Color.white;
        }

#endif
    }
}