using Code.Enemies.Core;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsCanAttack", story: "[Self] can go attackSlot", category: "Conditions", id: "05130e75ffaedf2e0eaf2d4c124e8273")]
public partial class IsCanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    public override bool IsTrue()
    {
        return Self.Value.BattleManager.CheckAttackSlotOccupant(Self);
    }
}
