using Code.Enemies;
using System;
using Code.Enemies.Core;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StateChange", message: "state change to [newValue]", category: "Events", id: "c5d0474b240047a8fb22e94dcb7360db")]
public sealed partial class StateChange : EventChannel<EnemyStateEnum> { }

