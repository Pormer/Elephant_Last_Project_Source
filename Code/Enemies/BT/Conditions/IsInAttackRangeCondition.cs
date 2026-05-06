using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsInAttackRange", story: "[Target] is in [Self] range [Distance]", category: "Conditions", id: "07c3541118feb11dab7db5141e658c52")]
public partial class IsInAttackRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Entity> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Distance;

    public override bool IsTrue()
    {
        float distance = Vector3.Distance(Target.Value.transform.position, Self.Value.transform.position);
        return distance <= Distance + Target.Value.CombatData.size / 2;
    }
}
