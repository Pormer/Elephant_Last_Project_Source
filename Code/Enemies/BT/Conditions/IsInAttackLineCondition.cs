using Code.Entities;
using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsInAttackLine", story: "[Target] is in [Self] line [Distance]", category: "Conditions", id: "ee14ea0358e3abd4f92a6f51e0e29775")]
public partial class IsInAttackLineCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Entity> Target;
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<float> Distance;

    private const float YZOffset = 0.25f;
    private const float XOffset = 0.1f;

    public override bool IsTrue()
    {
        Vector3 attackPos = Self.Value.GetTargetPos(Target.Value, Distance);
        Vector3 selfPos = Self.Value.transform.position;
        Vector3 targetPos = Target.Value.transform.position;
        Vector3 offset = attackPos - Self.Value.transform.position;
        
        // YZ 축으로 안에 있는지 확인
        bool isInYZ = MathF.Abs(offset.y) + MathF.Abs(offset.z) < YZOffset;
        // X 축으로 적과 공격 위치 사이에 있는지 확인
        bool isInX = (selfPos.x - XOffset <= attackPos.x && selfPos.x >= targetPos.x ||
                      selfPos.x + XOffset >= attackPos.x && selfPos.x <= targetPos.x); 
        
        return isInYZ && isInX;
    }
}

// 적이 목표와 같은 라인에 있고 특정 길이보다 가까이 있을 경우 체크하려고