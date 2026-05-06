using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTimerCondition", story: "check [Timer] pass [Sec]", category: "Conditions", id: "086f6d87aabb79942814f299a7274bce")]
public partial class CheckTimerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> Timer;
    [SerializeReference] public BlackboardVariable<float> Sec;

    public override bool IsTrue()
    {
        return Timer + Sec < Time.time;
    }
}
