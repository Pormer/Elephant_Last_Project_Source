using Code.Enemies.Core;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AddPassiveList", story: "[Self] add passiveList", category: "Action", id: "3ec7c7f2f8a1d2dbd737daa9f0fbc48f")]
public partial class AddPassiveListAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    protected override Status OnStart()
    {
        Self.Value.BattleManager.AddPassiveEnemy(Self);
        return Status.Success;
    }
}

