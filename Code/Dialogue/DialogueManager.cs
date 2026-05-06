using System;
using System.Collections.Generic;
using System.Linq;
using Code.Setting;
using EventSystem;
using GSheetConv.Runtime;
using Input;
using UnityEngine;

namespace Code.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO dialogueChannel;
        [SerializeField] private GameEventChannelSO settingChannel;
        [SerializeField] private CSVItemSO dialogueSheet;

        private bool _doesStop;
        private DialoguePositionData[] _positions;
        private LanguageType _playerLanguage;

        private string[] _choices;

        private const string GotoKey = "goto";
        private const string CharacterKey = "character";
        private const string AutoKey = "auto";
        private const int PlayerIndex = -1;

        private void Awake()
        {
            dialogueChannel.AddListener<DialogueStartRequestEvent>(HandleDialogueRequest);
            dialogueChannel.AddListener<DialogueInputEvent>(HandleDialogueInput);
            settingChannel.AddListener<LanguageChangeEvent>(HandleLanguageChange);
        }

        private void OnDestroy()
        {
            dialogueChannel.RemoveListener<DialogueStartRequestEvent>(HandleDialogueRequest);
            dialogueChannel.RemoveListener<DialogueInputEvent>(HandleDialogueInput);
            settingChannel.RemoveListener<LanguageChangeEvent>(HandleLanguageChange);
        }

        private void HandleLanguageChange(LanguageChangeEvent obj)
        {
            _playerLanguage  = obj.Language;
        }

        private void HandleDialogueRequest(DialogueStartRequestEvent obj)
        {
            _positions = obj.Position;
            ShowDialogue(obj.DialogueID, obj.Position);
        }

        private void HandleDialogueInput(DialogueInputEvent obj)
        {
            if (_choices == null || _choices.Length == 0)
            {
                dialogueChannel.RaiseEvent(DialogueEvents.DialogueEndEvent);
                return;
            }

            ShowDialogue(_choices[obj.ChoiceIndex], _positions);
        }

        private void ShowDialogue(string id, DialoguePositionData[] positionDatas)
        {
            var text = GetText(id);
            _choices = GetNextChoices(id, out var texts);
            var isAuto = GetIsAuto(id);

            if (!int.TryParse(dialogueSheet.GetValue(id, CharacterKey), out var characterIndex))
            {
                Debug.LogError(
                    $"[DialogueManager] Cannot parse {dialogueSheet.GetValue(id, CharacterKey)} to int!");
                return;
            }

            if (characterIndex == PlayerIndex)
            {
                if (_choices == null || _choices.Length == 0)
                    dialogueChannel.RaiseEvent(DialogueEvents.DialogueEndEvent);
                else
                    ShowDialogue(_choices[0], positionDatas);
                return;
            }

            if (positionDatas == null || characterIndex < 0 || characterIndex >= positionDatas.Length)
            {
                Debug.LogError(
                    $"[DialogueManager] Invalid character index {characterIndex} for dialogue {id}. Position count: {positionDatas?.Length ?? 0}");
                dialogueChannel.RaiseEvent(DialogueEvents.DialogueEndEvent);
                return;
            }

            dialogueChannel.RaiseEvent(
                DialogueEvents.DialogueShowEvent.Initializer(text, positionDatas[characterIndex], texts, isAuto));
        }

        private string GetText(string id)
        {
            var text = dialogueSheet.GetValue(id, _playerLanguage.ToString());
            return text;
        }

        private string[] GetNextChoices(string id, out string[] texts)
        {
            var nextId = dialogueSheet.GetValue(id, GotoKey).Trim();
            if (string.IsNullOrEmpty(nextId))
            {
                texts = null;
                return null;
            }

            var choices = Array.ConvertAll(nextId.Split('/'), p => p.Trim());
            if (choices.Length >= 2)
            {
                texts = new string[choices.Length];
                for (var i = 0; i < texts.Length; i++)
                    texts[i] = GetText(choices[i]);
                return choices;
            }

            if (!int.TryParse(dialogueSheet.GetValue(choices[0], CharacterKey), out int characterId))
            {
                Debug.LogError($"[DialogueManager] Cannot parse {choices[0]} to int!");
                texts = null;
                return null;
            }

            if (characterId == PlayerIndex)
                texts = new string[] { GetText(choices[0]) };
            else
                texts = null;
            return choices;
        }

        private bool GetIsAuto(string id)
        {
            if (bool.TryParse(dialogueSheet.GetValue(id, AutoKey), out bool isAuto)) return isAuto;
            Debug.LogError($"[DialogueManager] Cannot parse {dialogueSheet.GetValue(id, AutoKey)} to bool!");
            return false;
        }
    }
}