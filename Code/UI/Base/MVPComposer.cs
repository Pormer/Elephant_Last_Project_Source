using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    public class MVPComposer : MonoBehaviour
    {
        private void Awake()
        {
            var modelComponents = GetComponentsInChildren<ModelBase>(true);
            var models = new Dictionary<Type, ModelBase>(modelComponents.Length);
            var warnedTypes = new HashSet<Type>();

            for (var i = 0; i < modelComponents.Length; i++)
            {
                var currentModel = modelComponents[i];
                if (currentModel == null) continue;

                var modelType = currentModel.GetType();
                if (models.ContainsKey(modelType))
                {
                    if (warnedTypes.Add(modelType))
                        Debug.LogWarning($"Multiple {modelType} found in children. Using first instance.", this);
                    continue;
                }

                models.Add(modelType, currentModel);
            }

            var presenters = GetComponentsInChildren<PresenterBase>(true);
            for (var i = 0; i < presenters.Length; i++)
            {
                var presenter = presenters[i];
                if (presenter == null) continue;

                if (models.TryGetValue(presenter.ModelType, out var model))
                    presenter.Initialize(model);
                else
                    Debug.LogError($"{presenter.ModelType} not found");
            }
        }
    }
}
