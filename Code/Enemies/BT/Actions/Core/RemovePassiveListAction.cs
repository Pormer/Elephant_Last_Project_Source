using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RemovePassiveList", story: "[Self] remove passiveList", category: "Action", id: "75bda34b27c090e78c13ae6146d8c717")]
public partial class RemovePassiveListAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    protected override Status OnStart()
    {
        Self.Value.BattleManager.RemovePassiveEnemy(Self);
        return Status.Success;
    }
}

