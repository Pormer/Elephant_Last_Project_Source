using System;
using Code.Entities.Modules;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShakeVisual", story: "[Renderer] shake for [newValue] second", category: "Action", id: "72b5f9f2b1fe704c1ae95e3e92930d7e")]
public partial class ShakeVisualAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;
    [SerializeReference] public BlackboardVariable<float> NewValue;
    [SerializeReference] public BlackboardVariable<float> ShakeSpace;

    private bool _isMoved;
    private float _timer;
    private float _shakeTimer;
    
    protected override Status OnStart()
    {
        _isMoved = false;
        _timer = 0;
        _shakeTimer = 0;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _timer += Time.deltaTime;
        _shakeTimer += Time.deltaTime;

        if (_shakeTimer >= ShakeSpace)
        {
            _shakeTimer = 0;
            _isMoved = !_isMoved;
            MoveVisual(_isMoved ? 0.013f : -0.013f);
        }
        
        if (_timer >= NewValue)
        {
            MoveVisual(0);
            return Status.Success;
        }
        
        return Status.Running;
    }

    private void MoveVisual(float x)
        => Renderer.Value.transform.localPosition = new Vector3(x, 0, 0);
}