using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual
{
    public abstract class BaseStyle<T, TStyle> : MonoBehaviour, IStyle where T : Component where TStyle : BaseStyleData
    {
        [SerializeField] private TStyle defaultStyle;
        [SerializeField] protected TStyle[] styleDatas;

        protected readonly Dictionary<string, TStyle> _styles = new Dictionary<string, TStyle>();
        protected readonly Dictionary<string, int> _states = new Dictionary<string, int>();
        protected readonly HashSet<string> _appliedStates = new HashSet<string>();

        private readonly List<MultiStateEntry> _multiStates = new List<MultiStateEntry>();
        private readonly HashSet<string> _statesInMulti = new HashSet<string>();

        protected T target;

        private string _currentState;
        protected bool _isInitialized = false;

        public List<TStyle> GetStyles()
        {
            var res = new List<TStyle>();
            res.Add(defaultStyle);
            res.AddRange(styleDatas);
            return res;
        }

        private struct MultiStateEntry
        {
            public string Key;
            public string[] States;
        }

        private async void Start()
        {
            try
            {
                await UniTask.WaitForEndOfFrame();
                _isInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public virtual void Initialize(GameObject tar)
        {
            target = tar.GetComponent<T>();
            _styles.Clear();
            _states.Clear();
            _appliedStates.Clear();
            _multiStates.Clear();
            _statesInMulti.Clear();
            _currentState = null;

            if (styleDatas == null)
            {
                UpdateCurrentState().Forget();
                return;
            }

            for (var i = 0; i < styleDatas.Length; i++)
            {
                var styleData = styleDatas[i];
                if (styleData == null || string.IsNullOrEmpty(styleData.State)) continue;
                if (_styles.ContainsKey(styleData.State)) continue;

                _styles.Add(styleData.State, styleData);

                if (!styleData.State.Contains("&")) continue;

                var states = styleData.State.Split('&');
                _multiStates.Add(new MultiStateEntry
                {
                    Key = styleData.State,
                    States = states
                });

                for (var j = 0; j < states.Length; j++)
                {
                    if (!string.IsNullOrEmpty(states[j]))
                        _statesInMulti.Add(states[j]);
                }
            }

            UpdateCurrentState().Forget();
        }

        public async UniTask AddState(string state, int priority = 0)
        {
            if (string.IsNullOrEmpty(state)) return;
            _appliedStates.Add(state);

            if (!_styles.ContainsKey(state) && !_statesInMulti.Contains(state))
                return;

            if (_states.TryGetValue(state, out var currentPriority) && currentPriority == priority)
                return;

            _states[state] = priority;
            await UpdateCurrentState();
        }

        public async UniTask RemoveState(string state)
        {
            if (string.IsNullOrEmpty(state)) return;
            var removedApplied = _appliedStates.Remove(state);
            var removedState = _states.Remove(state);
            if (!removedApplied && !removedState) return;

            await UpdateCurrentState();
        }

        public async UniTask ClearStates()
        {
            if (_states.Count == 0 && _appliedStates.Count == 0)
                return;

            _states.Clear();
            _appliedStates.Clear();
            await UpdateCurrentState();
        }

        public async UniTask UpdateCurrentState()
        {
            if (target == null) return;

            if (_states.Count <= 0)
            {
                if (_currentState == "") return;
                _currentState = "";
                await ApplyStyle(defaultStyle);
                return;
            }

            var highestPriority = int.MinValue;
            string selectedState = null;

            foreach (var state in _states)
            {
                if (!_styles.ContainsKey(state.Key)) continue;
                if (state.Value <= highestPriority) continue;
                highestPriority = state.Value;
                selectedState = state.Key;
            }

            if (CheckMultipleState(selectedState, out var highestPriorityState))
                selectedState = highestPriorityState;

            if (string.IsNullOrEmpty(selectedState))
            {
                if (_currentState == "") return;
                _currentState = "";
                await ApplyStyle(defaultStyle);
                return;
            }

            if (_currentState == selectedState) return;

            _currentState = selectedState;
            await ApplyStyle(_styles[selectedState]);
        }

        private bool CheckMultipleState(string selectedState, out string result)
        {
            result = selectedState;
            var maxPriority = int.MinValue;
            string bestMultiStateKey = null;

            for (var i = 0; i < _multiStates.Count; i++)
            {
                var multiState = _multiStates[i];
                var states = multiState.States;
                var currentPrioritySum = 0;
                var allExist = true;

                for (var j = 0; j < states.Length; j++)
                {
                    var stateKey = states[j];
                    if (_states.TryGetValue(stateKey, out var priority))
                    {
                        currentPrioritySum += priority;
                    }
                    else if (_appliedStates.Contains(stateKey))
                    {
                        currentPrioritySum += 1;
                    }
                    else
                    {
                        allExist = false;
                        break;
                    }
                }

                if (!allExist) continue;
                if (currentPrioritySum <= maxPriority) continue;

                maxPriority = currentPrioritySum;
                bestMultiStateKey = multiState.Key;
            }

            if (bestMultiStateKey == null || maxPriority <= 0) return false;

            result = bestMultiStateKey;
            return true;
        }

        protected abstract UniTask ApplyStyle(TStyle currentState);
    }

    [Serializable]
    public abstract class BaseStyleData
    {
        [Tooltip("Name of the State. If multiple states are applied, the one with the highest priority will take effect." +
                 "If you want to apply style when multiple states are active, name it by joining state names with <b>'&'</b>, e.g. 'state1&state2'")]
        public string State = "default";
        public Ease Ease = Ease.OutQuint;
        public float Duration = 0.125f;
    }
}
