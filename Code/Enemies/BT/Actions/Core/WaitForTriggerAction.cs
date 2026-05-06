using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForTrigger", story: "wait for [Trigger] end", category: "Action", id: "3911f1082e427fc22ef5fea2e39497a8")]
public partial class WaitForTriggerAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

    private bool _isTriggered;
        
    protected override Status OnStart()
    {
        _isTriggered = false;
        Trigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return _isTriggered ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;
    }

    private void HandleAnimationEnd() => _isTriggered = true;
}

