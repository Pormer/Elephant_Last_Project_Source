using Code.Entities;
using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToRandomDir", story: "[Movement] move random dir [Min] to [Max] second [DirType]", category: "Action", id: "4c5541f7e69a8aa99ef6da7afda11462")]
public partial class MoveToRandomDirAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;
    [SerializeReference] public BlackboardVariable<RandomDirType> DirType;
    private Vector2 _moveDir;
    private float _moveTime;
    private float _timer;

    protected override Status OnStart()
    {
        _timer = 0;
        // 몇 초동안 어느 방향으로 이동할지 랜덤으로 가지고 옴
        _moveTime = Random.Range(Min.Value, Max.Value);
        _moveDir = Random.insideUnitCircle.normalized;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Movement.Value.SetMoveDirection(_moveDir);
        _timer += Time.deltaTime;

        return _timer >= _moveTime ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
    }

    [BlackboardEnum]
    public enum RandomDirType
    {
        Front,
        Back,
        Random,
    }
}