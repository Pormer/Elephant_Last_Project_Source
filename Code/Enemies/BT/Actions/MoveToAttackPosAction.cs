using Code.Entities;
using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToAttackPos", story: "[Movement] to [Target] by [Distance]", category: "Action", id: "f73a36be12d51e1548880f90e2d8f07c")]
public partial class MoveToAttackPosAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<Entity> Target;
    [SerializeReference] public BlackboardVariable<float> Distance;
    protected override Status OnStart()
    {
        Entity entity = Movement.Value.Entity; 
        Vector3 targetPos = entity.GetTargetPos(Target.Value, Distance.Value);

        Vector3 moveDir = (targetPos - entity.transform.position).normalized;
        Movement.Value.SetMoveDirection(moveDir);
        return Status.Success;
    }
    
    
}

