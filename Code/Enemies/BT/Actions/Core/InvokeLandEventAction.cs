using Code.Entities.Modules;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InvokeLandEvent", story: "invoke [Movement] landEvent", category: "Action", id: "8345ff9fb61e70ef0e2c30afcb7a8761")]
public partial class InvokeLandEventAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;

    protected override Status OnStart()
    {
        Movement.Value.OnLandedEvent?.Invoke();
        return Status.Success;
    }
}

