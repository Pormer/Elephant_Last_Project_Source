using Code.Entities.Modules;
using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DestroyEnemy", story: "destroy [Self] with [Renderer]", category: "Action", id: "f9913939de66dc90c741f29efd90d904")]
public partial class DestroyEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;
    
    protected override Status OnStart()
    {
        Destroy();
        return Status.Success;
    }

    private async void Destroy()
    {
        try
        {
            await Renderer.Value.StartDestroyEffect();
            Self.Value.DestroySelf();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}

