using System;
using System.Collections.Generic;
using System.Linq;
using Code.Modules;
using UnityEngine;

namespace Code.Players.Modules
{
    public struct BufferInputData
    {
        public string StateName { get; private set; }
        public float InputTime { get; private set; }
        public int Priority { get; private set; }

        public BufferInputData(string stateName, float inputTime, int priority)
        {
            StateName = stateName;
            InputTime = inputTime;
            Priority = priority;
        }
    }

    public class PlayerPreInputModule : MonoBehaviour, IModule
    {
        [SerializeField] private float bufferWindow = 0.3f; // 유효 시간
        [SerializeField] private int maxSameStateCount = 3;
        [SerializeField] private float inputMinSpacing = 0.02f;
        
        public string LastChangeStateName { get; private set; }
        public string NextChangeStateName => _inputBuffer.FirstOrDefault().StateName ?? string.Empty; 

        private Player _player;
        private List<BufferInputData> _inputBuffer;

        public void Initialize(ModuleOwner owner)
        {
            _inputBuffer = new List<BufferInputData>();
            _player = owner as Player;

            Debug.Assert(_player != null, $"{owner.name} is not a player");
        }

        public bool CheckNextAction(out string stateName)
        {
            while (_inputBuffer.Count > 0)
            {
                var nextData = _inputBuffer[0];
                _inputBuffer.RemoveAt(0);

                if (Time.time - nextData.InputTime <= bufferWindow)
                {
                    stateName = nextData.StateName;
                    LastChangeStateName = stateName;
                    return true;
                }
            }

            stateName = string.Empty;
            return false;
        }

        public void AddToBuffer(string stateName, int priority, bool canOverlapState = true)
        {
            float currentTime = Time.time;
            int sameStateCount = 0;
            bool alreadyExists = false;
            bool isTooFast = false;

            foreach (var data in _inputBuffer)
            {
                if (data.StateName.Equals(stateName, StringComparison.Ordinal))
                {
                    alreadyExists = true;
                    sameStateCount++;
                }

                if (Mathf.Abs(currentTime - data.InputTime) < inputMinSpacing)
                {
                    isTooFast = true;
                }
            }

            if (!canOverlapState && alreadyExists) return;
            if (sameStateCount >= maxSameStateCount) return;
            if (isTooFast) return;

            var newData = new BufferInputData(stateName, currentTime, priority);

            int insertIndex = _inputBuffer.Count;
            for (int i = 0; i < _inputBuffer.Count; i++)
            {
                if (newData.Priority < _inputBuffer[i].Priority)
                {
                    insertIndex = i;
                    break;
                }
            }
            
            _inputBuffer.Insert(insertIndex, newData);
        }

        public void ClearActionBuffer()
        {
            _inputBuffer.Clear();
        }
    }
}