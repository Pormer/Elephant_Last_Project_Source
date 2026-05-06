using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Modules
{
    public abstract class ModuleOwner : MonoBehaviour
    {
        protected Dictionary<Type, IModule> _moduleDict;

        protected virtual void Awake()
        {
            Initialize();
            AfterInitialize();
        }

        public virtual void Initialize()
        {
            _moduleDict = GetComponentsInChildren<IModule>().ToDictionary(module => module.GetType());
            _moduleDict.Values.ToList().ForEach(module => module.Initialize(this));
        }
        
        protected virtual void AfterInitialize()
        {
            _moduleDict.Values.OfType<IAfterInitModule>().ToList().ForEach(module => module.AfterInitialize());
        }
        
        public T GetModule<T>() where T : IModule
        {
            if (_moduleDict.TryGetValue(typeof(T), out IModule module))
            {
                return (T)module;
            }

            // 만약에 T가 인터페이스를 구현하거나 상속 받은 애들까지 가지고와야하면 케스트 시도
            IModule findModule = _moduleDict.Values.FirstOrDefault(moduleType => moduleType is T);
            
            if(findModule != null && findModule is T castedModule)
                return castedModule;

            return default;
        }

        public IModule GetModule(Type type)
        {
            if (_moduleDict.TryGetValue(type, out IModule module))
            {
                return module;
            }

            IModule findModule = _moduleDict.Values.FirstOrDefault(m => type.IsAssignableFrom(m.GetType()));

            return findModule;
        }
    }
}