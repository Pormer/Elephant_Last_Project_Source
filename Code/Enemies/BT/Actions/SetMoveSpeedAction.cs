using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetMoveSpeed", story: "set [Movement] moveSpeed to [NewValue]", category: "Action", id: "8e6d3bd8e263ff3ac95de14468f6fb8d")]
public partial class SetMoveSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<float> NewValue;

    protected override Status OnStart()
    {
        Movement.Value.SetSpeedMultiplier(NewValue.Value);
        return Status.Success;
    }
}

