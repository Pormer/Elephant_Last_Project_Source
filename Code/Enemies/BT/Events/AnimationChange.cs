using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AnimationChange", message: "change to [newAnimation]", category: "Events", id: "3f5e1a307fc2998815090744bf80e6a7")]
public sealed partial class AnimationChange : EventChannel<string> { }

