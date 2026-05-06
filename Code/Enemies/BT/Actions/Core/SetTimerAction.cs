using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetTimerAction", story: "Set time to [Timer] by [Offset]", category: "Action", id: "4caf18a1d3f09e4e7c02a02aa4186b56")]
public partial class SetTimerAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Timer;
    [SerializeReference] public BlackboardVariable<float> Offset;

    protected override Status OnStart()
    {
        Timer.Value = Time.time + Random.Range(-Offset, Offset);
        return Status.Success;
    }
}

