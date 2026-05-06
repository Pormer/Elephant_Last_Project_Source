using System.Collections.Generic;
using Code.Enemies.Core;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Combat
{
    [Provide]
    public class BattleManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private float selectPercent = 65f; // 순회하면서 먼저 온 적이 공격하러 갈 경우 
        
        private LinkedList<Enemy> _passiveEnemies = new LinkedList<Enemy>();
        private Enemy _attackSlot;

        public void AddPassiveEnemy(Enemy enemy)
        {
            if (_passiveEnemies.Contains(enemy))
            {
                Debug.Log("이미 대기중인 적임");
                return;
            }
            
            _passiveEnemies.AddLast(enemy);

            // 만약 추가된 대기 적이 공격슬롯에서 나오는 적일 경우 공격 슬롯 업데이트     
            if (_attackSlot == enemy || _attackSlot is null)
                UpdateAttackSlot();
        }

        public void RemovePassiveEnemy(Enemy enemy)
        {
            _passiveEnemies.Remove(enemy);
            if (_attackSlot == enemy)
                _attackSlot = null;
        }

        private void UpdateAttackSlot()
        {
            Enemy selectedEnemy = null;
            
            foreach (var enemy in _passiveEnemies)
            {
                if (Random.Range(0f, 100f) < selectPercent)
                {
                    selectedEnemy = enemy; 
                    break;
                }
            }

            // 선회를 전부 했는데도 선탠된 적이 없다면 마지막 적으로 고르기
            if (selectedEnemy is null)
                selectedEnemy = _passiveEnemies.Last.Value;

            RemovePassiveEnemy(selectedEnemy);
            _attackSlot = selectedEnemy;
        }

        public bool CheckAttackSlotOccupant(Enemy enemy) => _attackSlot == enemy;
    }
}