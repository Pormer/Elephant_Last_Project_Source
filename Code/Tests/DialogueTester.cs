using System;
using Code.Dialogue;
using EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Tests
{
    public class DialogueTester : MonoBehaviour
    {
        [SerializeField] private string dialogueId;
        [SerializeField] private string dialogueId1;
        [SerializeField] private DialoguePositionData[] positionDatas;
        [SerializeField] private GameEventChannelSO dialogueChannel;

        [Header("Interaction Boxes")] [SerializeField]
        private string interactionActionName;

        [SerializeField] private Transform target;
        [SerializeField] private GameEventChannelSO uiChannel;

        public void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
                dialogueChannel.RaiseEvent(
                    DialogueEvents.DialogueStartRequestEvent.Initializer(dialogueId, positionDatas));
            if (Keyboard.current.oKey.wasPressedThisFrame)
                dialogueChannel.RaiseEvent(
                    DialogueEvents.DialogueStartRequestEvent.Initializer(dialogueId1, positionDatas));
            if (Keyboard.current.iKey.wasPressedThisFrame)
                uiChannel.RaiseEvent(UIEvents.InteractionShowEvent.Initializer(interactionActionName, target));
            if (Keyboard.current.uKey.wasPressedThisFrame)
                uiChannel.RaiseEvent(UIEvents.InteractionHideEvent.Initializer(target));
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (positionDatas != null)
            {
                Gizmos.color = Color.red;
                foreach (var positionData in positionDatas)
                {
                    Gizmos.DrawSphere(positionData.Transform.position + Vector3.up * positionData.Height, 0.1f);
                }
            }
        }
#endif
    }
}