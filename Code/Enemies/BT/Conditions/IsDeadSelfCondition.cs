using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsDeadSelf", story: "[Self] is dead now", category: "Conditions", id: "9ea92dfd497b5aa3457130462af7f399")]
public partial class IsDeadSelfCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Entity> Self;

    public override bool IsTrue()
    {
        return Self.Value.IsDead;
    }
}
