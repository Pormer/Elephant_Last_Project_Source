using UnityEngine;

namespace Code.Combat.Attacks
{
    public interface IAttackable //공격의 주체가 될 수 있는 인터페이스
    {
        Transform AttackerTrm { get; }
    }
}