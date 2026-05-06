using Code.Entities.Modules;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsLanding", story: "is [Movement] landing ground", category: "Conditions", id: "ac46cf14c962a15bbbeca1e5a2bb1951")]
public partial class IsLandingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;

    public override bool IsTrue()
    {
        bool isLanding = Movement.Value.VerticalVelocity <= 0 && Movement.Value.IsGrounded;
        return isLanding;
    }
}
