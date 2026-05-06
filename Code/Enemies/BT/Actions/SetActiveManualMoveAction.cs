using Code.Entities.Modules;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetActiveManualMove", story: "set active [Movement] manualMove to [NewValue]", category: "Action", id: "8cafebed88ecba13109c67d183f5d3a2")]
public partial class SetActiveManualMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<bool> NewValue;

    protected override Status OnStart()
    {
        Movement.Value.SetCanManualMove(NewValue);
        return Status.Success;
    }
}

