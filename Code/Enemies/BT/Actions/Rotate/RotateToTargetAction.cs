using Code.Entities;
using Code.Entities.Modules;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateToTarget", story: "rotate [Renderer] to [Target]", category: "Action", id: "10fc8d9818197c32a472e23984192624")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;
    [SerializeReference] public BlackboardVariable<Entity> Target;

    protected override Status OnStart()
    {
        bool isRight = Target.Value.transform.position.x > Renderer.Value.Owner.transform.position.x;
        Renderer.Value.SetFacingRight(isRight);
        return Status.Success;
    }
}

