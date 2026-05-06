using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckRandomTimer", story: "check [Timer] pass [Min] to [Max] second", category: "Conditions", id: "d7a838d34a55237707326b40e678dd92")]
public partial class CheckRandomTimerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
