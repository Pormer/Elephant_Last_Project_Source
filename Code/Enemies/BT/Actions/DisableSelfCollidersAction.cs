using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DisableSelfColliders", story: "disable [Self] colliders", category: "Action", id: "92755de523b93446c5aee80863208759")]
public partial class DisableSelfCollidersAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    protected override Status OnStart()
    {
        Self.Value.DisableCollider();
        return Status.Success;
    }
}

