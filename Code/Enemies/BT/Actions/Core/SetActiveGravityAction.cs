using Code.Entities.Modules;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetActiveGravity", story: "set active [Movement] gravity to [NewValue]", category: "Action", id: "690266d31503d82266ed98bf2f60a993")]
public partial class SetActiveGravityAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<bool> NewValue;

    protected override Status OnStart()
    {
        Movement.Value.SetUseGravity(NewValue);
        return Status.Success;
    }
}

