using EventSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using Input;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.UI.InGame.Dialogue
{
    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] private PoolItemSO dialogueItem;
        [SerializeField] private GameEventChannelSO dialogueChannel;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private UIInputSO uiInput;
        [Inject] private PoolManagerMono _poolManager;

        private Transform _currentSpeaking;
        private DialogueBox _currentDialogueBox;

        private void Awake()
        {
            dialogueChannel.AddListener<DialogueShowEvent>(HandleDialogueShow);
            dialogueChannel.AddListener<DialogueEndEvent>(HandleDialogueEnd);
        }

        private void OnDestroy()
        {
            dialogueChannel.RemoveListener<DialogueShowEvent>(HandleDialogueShow);
            dialogueChannel.RemoveListener<DialogueEndEvent>(HandleDialogueEnd);
        }

        private void HandleDialogueShow(DialogueShowEvent obj)
        {
            if (_currentSpeaking == obj.Position.Transform)
            {
                _currentDialogueBox.SetDialogue(obj.Text, obj.Position.Transform, obj.Position.Height, obj.Auto,
                    obj.ChoicesText);
                uiChannel.RaiseEvent(UIEvents.InputChangeEvent.Initializer(obj.Auto, !obj.Auto));
                return;
            }

            if (_currentDialogueBox != null)
            {
                _currentDialogueBox.OnChoice -= HandleChoice;
                _currentDialogueBox.EndDialogue();
            }

            var dialogue = _poolManager.Pop<DialogueBox>(dialogueItem);
            dialogue.transform.SetParent(transform);
            dialogue.SetDialogue(obj.Text, obj.Position.Transform, obj.Position.Height, obj.Auto, obj.ChoicesText);
            uiChannel.RaiseEvent(UIEvents.InputChangeEvent.Initializer(obj.Auto, !obj.Auto));
            dialogue.OnChoice += HandleChoice;

            _currentSpeaking = obj.Position.Transform;
            _currentDialogueBox = dialogue;
        }

        private void HandleChoice(int idx, DialogueBox dialogueBox)
        {
            dialogueBox.OnChoice -= HandleChoice;
            dialogueChannel.RaiseEvent(DialogueEvents.DialogueInputEvent.Initializer(idx));
        }

        private void HandleDialogueEnd(DialogueEndEvent obj)
        {
            _currentSpeaking = null;
            if (_currentDialogueBox != null)
            {
                _currentDialogueBox.OnChoice -= HandleChoice;
                _currentDialogueBox.EndDialogue();
            }
            _currentDialogueBox = null;
            uiChannel.RaiseEvent(UIEvents.InputChangeEvent.Initializer(true, true));
        }
    }
}
