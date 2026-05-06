using Code.Entities.Modules;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateToMoveDir", story: "rotate [Renderer] to [Movement] dir", category: "Action", id: "601cca46dc460fef9b450fc08f053ce4")]
public partial class RotateToMoveDirAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;

    protected override Status OnStart()
    {
        if (Movement.Value.CurrentMoveDirection.x != 0)
        { 
            Renderer.Value.SetFacingRight(Movement.Value.CurrentMoveDirection.x > 0);
        }
        
        return Status.Success;
    }
}

