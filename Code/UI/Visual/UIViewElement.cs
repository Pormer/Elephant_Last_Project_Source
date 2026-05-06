using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Visual
{
    public class UIViewElement : MonoBehaviour
    {
        [SerializeField] private Transform[] styleContainers;
        [SerializeField] private UIViewElement[] childElements;
        private readonly HashSet<string> _styleClasses = new HashSet<string>();
        private readonly List<IStyle> _styles = new List<IStyle>();

        private void Awake()
        {
            _styles.Clear();
            if (styleContainers == null) return;

            for (var i = 0; i < styleContainers.Length; i++)
            {
                var container = styleContainers[i];
                if (container == null) continue;
                var styleComponents = container.GetComponents<IStyle>();
                for (var j = 0; j < styleComponents.Length; j++)
                {
                    var styleComponent = styleComponents[j];
                    styleComponent.Initialize(gameObject);
                }

                _styles.AddRange(styleComponents);
            }
        }

        public async UniTask AddState(string state, int priority = 1, bool waitForChildren = false)
        {
            if (!_styleClasses.Add(state))
                return;
            if (childElements != null && childElements.Length > 0)
            {
                if (waitForChildren)
                {
                    for (var i = 0; i < childElements.Length; i++)
                    {
                        var child = childElements[i];
                        if (child == null) continue;
                        await child.AddState(state, priority, true);
                    }
                }
                else
                {
                    for (var i = 0; i < childElements.Length; i++)
                    {
                        var child = childElements[i];
                        if (child == null) continue;
                        child.AddState(state, priority, false).Forget();
                    }
                }
            }

            if (_styles.Count == 0)
                return;

            List<UniTask> tasks = new List<UniTask>();
            for (var i = 0; i < _styles.Count; i++)
            {
                var style = _styles[i];
                if (style == null) continue;
                if (waitForChildren)
                    await style.AddState(state, priority);
                else
                    tasks.Add(style.AddState(state, priority));
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask RemoveState(string state, bool waitForChildren = false)
        {
            if (!_styleClasses.Remove(state))
                return;

            if (childElements != null && childElements.Length > 0)
            {
                if (waitForChildren)
                {
                    for (var i = 0; i < childElements.Length; i++)
                    {
                        var child = childElements[i];
                        if (child == null) continue;
                        await child.RemoveState(state, true);
                    }
                }
                else
                {
                    for (var i = 0; i < childElements.Length; i++)
                    {
                        var child = childElements[i];
                        if (child == null) continue;
                        child.RemoveState(state, false).Forget();
                    }
                }
            }

            if (_styles.Count == 0)
                return;

            List<UniTask> tasks = new List<UniTask>();
            for (var i = 0; i < _styles.Count; i++)
            {
                var style = _styles[i];
                if (style == null) continue;
                if (waitForChildren)
                    await style.RemoveState(state);
                else
                    tasks.Add(style.RemoveState(state));
            }

            await UniTask.WhenAll(tasks);
        }

        public bool HasState(string state)
        {
            return _styleClasses.Contains(state);
        }

        public async UniTask ClearStates()
        {
            while (_styleClasses.Count > 0)
            {
                using var enumerator = _styleClasses.GetEnumerator();
                if (!enumerator.MoveNext()) break;
                var state = enumerator.Current;
                await RemoveState(state, true);
            }
        }
    }
}