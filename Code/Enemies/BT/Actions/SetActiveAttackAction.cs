using Code.Enemies.Core.Attack;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetActiveAttack", story: "set active [Attack] by [IsActive]", category: "Action", id: "d8b33843e5136e0696c44956c1ca8883")]
public partial class SetActiveAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyAttackController> Attack;
    [SerializeReference] public BlackboardVariable<bool> IsActive;

    protected override Status OnStart()
    {
        if (IsActive)   
            Attack.Value.StartAttack();
        else
            Attack.Value.EndAttack();
        
        return Status.Success;
    }
}

