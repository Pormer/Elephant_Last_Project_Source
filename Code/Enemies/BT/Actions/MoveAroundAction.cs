using Code.Entities;
using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveAround", story: "[Movement] move around [Target] between [Min] to [Max]", category: "Action", id: "a1579ec76bafd642cf72690c11d92982")]
public partial class MoveAroundAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityMovement> Movement;
    [SerializeReference] public BlackboardVariable<Entity> Target;
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;
    [SerializeReference] public BlackboardVariable<float> BaseAroundDistance;
    [SerializeReference] public BlackboardVariable<float> DistanceOffset;
    [SerializeReference] public BlackboardVariable<float> OrbitSpeed;

    private Transform _selTrm;
    private Vector3 _targetPos;

    private float _duration;
    private float _timer;
    private float _aroundDistance;
    private float _aroundDirection; // 왼쪽으로 돌지 오른쪽으로 돌지 결정하는 방향
    private float _currentAngle;    // 현재 가상의 구의 각도

    protected override Status OnStart()
    {
        _duration = Random.Range(Min, Max);
        _timer = 0;
        _selTrm = Movement.Value.Entity.transform;
        _targetPos = Target.Value.transform.position;
        
        // 왼쪽으로 돌지 오른쪽으로 돌지 결정
        _aroundDirection = Random.value < 0.5f ? -1f : 1f;
        // Offset만큼 랜덤하게 약간 크기 조절
        _aroundDistance = BaseAroundDistance + Random.Range(-DistanceOffset, DistanceOffset);
        
        // 처음 자신의 각도를 기준으로 회전 시작
        Vector3 direction = _selTrm.position - Target.Value.transform.position;
        _currentAngle = Mathf.Atan2(direction.z, direction.x);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 direction = GetNextOrbitPos() - _selTrm.position;
        direction.Normalize();
        Movement.Value.SetMoveDirection(direction);
     
        _timer += Time.deltaTime;
        if (_timer >= _duration)
            return Status.Success;
        
        return Status.Running;
    }

    private Vector3 GetNextOrbitPos()
    {
        _currentAngle += OrbitSpeed * Time.deltaTime * _aroundDirection;
        float x = Mathf.Cos(_currentAngle) * _aroundDistance;
        float z = Mathf.Sin(_currentAngle) * _aroundDistance;
        
        return _targetPos + new Vector3(x, 0, z);
    }
}

