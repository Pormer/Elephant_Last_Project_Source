using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.FSM
{
    public class EntityStateMachine
    {
        public bool CanStateChangeable { get; set; } = true;
        public EntityState CurrentState { get; private set; }
        public string CurrentStateName { get; private set; }
        
        private EntityState _prevState;

        private Dictionary<string, EntityState> _states;
        private Entity _owner;

        public EntityStateMachine(Entity entity, StateDataSO[] stateList)
        {
            _states = new Dictionary<string, EntityState>();
            _owner = entity;
            
            foreach (StateDataSO state in stateList)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null : {state.className}");
                EntityState entityState = Activator.CreateInstance(type, entity, state.animationHash)
                                        as EntityState;
                _states.Add(state.stateName, entityState);
            }
        }

        public void ChangeState(string newStateName, bool isForce = false)
        {
            if(CanStateChangeable == false && isForce == false) return;
            
            EntityState newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != null, $"State is null {newStateName}");
            
            if(CurrentState != null)
            {
                bool isSameState = CurrentState.Equals(newState);
                if (isSameState && !isForce) return;
            }
            
            CurrentState?.Exit();
            
            _prevState = CurrentState;
            CurrentState = newState;
            CurrentStateName = newStateName;
            
            CurrentState.Enter();
        }

        public void UpdateStateMachine()
        {
            CurrentState?.Update();
        }
        
        public void FixedUpdateStateMachine()
        {
            CurrentState?.FixedUpdate();
        }

        public EntityState GetCurrentState() => CurrentState;

        public bool IsCurrentState(string stateName)
        {
            return CurrentStateName.Equals(stateName, StringComparison.Ordinal);
        }
    }
}