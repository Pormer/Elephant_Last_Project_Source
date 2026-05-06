using System;
using Code.Enemies.Core;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsAtAttackPos", story: "[Self] at [Target] attackPos in [distance]", category: "Conditions", id: "51e46bc8cf4a3cbab524e467f9f7a2d7")]
public partial class IsAtAttackPosCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<Entity> Target;
    [SerializeReference] public BlackboardVariable<float> Distance;

    public override bool IsTrue()
    {
        var targetPos = Self.Value.GetTargetPos(Target.Value, Distance.Value); 
        targetPos.y = Self.Value.transform.position.y;
        
        float distance = Vector3.Distance(targetPos, Self.Value.transform.position);
        return distance < 0.15f;
    }
}

// 공격 위치에 완전히 왔을 때 사용하려고