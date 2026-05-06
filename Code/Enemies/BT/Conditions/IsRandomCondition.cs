using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsRandomCondition", story: "is true for [Random]", category: "Conditions", id: "fc235bb63460662daffdcdfc3eb87535")]
public partial class IsRandomCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> Random;
    
    public override bool IsTrue()
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        return randomValue <= Random.Value;
    }
}
