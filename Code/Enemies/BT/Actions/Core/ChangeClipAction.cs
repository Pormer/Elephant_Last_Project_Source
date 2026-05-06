using System;
using Code.Entities;
using Code.Entities.Modules;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangeClip", story: "[MainAnimator] change [oldClip] to [newClip]", category: "Action", id: "72380489d580bc1275058584a2435836")]
public partial class ChangeClipAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityAnimator> MainAnimator;
    [SerializeReference] public BlackboardVariable<string> OldClip;
    [SerializeReference] public BlackboardVariable<string> NewClip;

    protected override Status OnStart()
    {
        int oldHash = Animator.StringToHash(OldClip.Value);
        int newHash = Animator.StringToHash(NewClip.Value);
        MainAnimator.Value.SetParam(oldHash, false);
        MainAnimator.Value.SetParam(newHash, true);
        
        OldClip.Value = NewClip.Value;
        return Status.Success;
    }
}