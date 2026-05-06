using System;
using System.Collections.Generic;
using Code.Enemies;
using Code.Enemies.Core;
using Code.Modules;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetModulesAction", story: "get modules from [Self]", category: "Action", id: "b29755d340dae0358b82fed716c24b86")]
public partial class GetModulesAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;

    protected override Status OnStart()
    {
        Enemy enemy = Self.Value;
        SetVariable(enemy, "Target", enemy.PlayerFinder.Target);

        List<BlackboardVariable> varList = enemy.BtAgent.BlackboardReference.Blackboard.Variables;

        foreach (BlackboardVariable variable in varList)
        {
            if(typeof(IModule).IsAssignableFrom(variable.Type) == false) continue;
                
            SetModules(enemy, variable.Name, enemy.GetModule(variable.Type));
        }
            
        return Status.Success;
    }

    private void SetModules(Enemy enemy, string varName, IModule module)
    {
        if (enemy.BtAgent.GetVariable(varName, out BlackboardVariable variable))
        {
            variable.ObjectValue = module;
        }
    }
        
    private void SetVariable<T>(Enemy enemy, string varName, T component)
    {
        Debug.Assert(component != null, $"Check {varName} in {enemy.name}");
        BlackboardVariable<T> target = enemy.GetBlackboardVariable<T>(varName);
        target.Value = component;
    }
}