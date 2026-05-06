using Code.Entities;
using Code.Entities.Modules;
using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "[Movement] move [Self] to [Target]", category: "Action", id: "3905f81a99f541f2d3d1241506ab5ffd")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<Entity> Target;

    protected override Status OnStart()
    {
        Vector3 direction = Target.Value.transform.position - Self.Value.transform.position;
        Movement.Value.SetMoveDirection(direction.normalized);
        return Status.Success;
    }
}

