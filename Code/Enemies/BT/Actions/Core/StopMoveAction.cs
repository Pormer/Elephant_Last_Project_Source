using Code.Entities;
using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopMove", story: "stop [Movement]", category: "Action", id: "59f51a8bfe7906c921fa84882c04a063")]
public partial class StopMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;

    protected override Status OnStart()
    {
        Movement.Value.StopMovement();
        return Status.Success;
    }
}

