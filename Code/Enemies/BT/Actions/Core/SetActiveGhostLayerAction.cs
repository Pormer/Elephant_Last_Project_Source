using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetActiveGhostLayer", story: "set active ghost layer [Self] to [NewValue]", category: "Action", id: "723664ad953ad40c863b0936134de057")]
public partial class SetActiveGhostLayerAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<bool> NewValue;

    protected override Status OnStart()
    {
        Self.Value.SetGhostLayer(NewValue);
        return Status.Success;
    }
}

