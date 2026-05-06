using Code.Entities.Modules;
using System;
using Code.Enemies.Core;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "JumpByType", story: "[Movement] jump by [JumpType] by [JumpPower]", category: "Action", id: "7ff6fcc0e1e6c0c1b4cb53c696a2f0b4")]
public partial class JumpByTypeAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<JumpDirType> JumpType;
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<Entity> Target;
    [SerializeReference] public BlackboardVariable<float> JumpPower;
    [SerializeReference] public BlackboardVariable<float> HorizontalPower;

    protected override Status OnStart()
    {
        switch (JumpType.Value)
        {
            case JumpDirType.NoneDir:
                Movement.Value.Jump();
                break;
            case JumpDirType.RandomDir:
                RandomDirJump();
                break;
            case JumpDirType.TargetDir:
                TargetDirJump();
                break;
        }
        
        return Status.Success;
    }

    private void RandomDirJump()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        direction *= HorizontalPower;
        Movement.Value.JumpTo(new Vector3(direction.x, 0, direction.y), JumpPower);
        Movement.Value.SetMoveDirection(direction);
    }
    
    private void TargetDirJump()
    {
        Vector3 direction = (Target.Value.transform.position - Self.Value.transform.position).normalized;
        direction *= HorizontalPower;
        Movement.Value.JumpTo(direction, JumpPower);
        Movement.Value.SetMoveDirection(direction);
    }
    
    [BlackboardEnum]
    public enum JumpDirType
    {
        NoneDir,
        RandomDir,
        TargetDir
    }
}

