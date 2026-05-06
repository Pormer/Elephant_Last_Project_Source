using System;
using System.Collections.Generic;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using Input;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Dialogue
{
    public class ChoiceList : MonoBehaviour
    {
        [SerializeField] private UIViewElement element;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private GameObject nextImage;
        private List<Choice> _choices = new List<Choice>();

        private const string HideKey = "hide";

        public Action<int> ChoicePressed;
        public Action SkipPressed;

        public void ReadyForEnd()
        {
            foreach (var c in _choices)
                c.gameObject.SetActive(false);
            element.AddState(HideKey).Forget();
            uiInput.OnSubmitPressed -= HandleNext;
            uiInput.OnSubmitPressed += HandleSkip;
            nextImage.SetActive(false);
        }

        public void SetChoiceList(string[] choices)
        {
            uiInput.OnSubmitPressed -= HandleSkip;
            if (choices == null || choices.Length == 0)
            {
                uiInput.OnSubmitPressed += HandleNext;
                nextImage.SetActive(true);
                return;
            }


            for (int i = 0; i < choices.Length; i++)
            {
                if (_choices.Count <= i)
                {
                    var nc = Instantiate(choicePrefab, transform).GetComponent<Choice>();
                    nc.transform.rotation = transform.rotation;
                    _choices.Add(nc);
                    nc.Initialize();
                    nc.OnPressed += HandlePressed;
                }

                _choices[i].gameObject.SetActive(true);
                _choices[i].SetText(choices[i], i);
            }

            element.RemoveState(HideKey).Forget();
            _choices[0].Pointer.Select();
        }

        public void ClearChoiceList()
        {
            uiInput.OnSubmitPressed -= HandleNext;
        }

        private void HandleSkip()
        {
            SkipPressed?.Invoke();
        }

        private void HandleNext()
        {
            ChoicePressed?.Invoke(0);
        }

        private void HandlePressed(int obj)
        {
            ChoicePressed?.Invoke(obj);
        }
    }
}