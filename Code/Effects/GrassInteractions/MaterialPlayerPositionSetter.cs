using System;
using Code.Players;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Effects.GrassInteractions
{
    public class MaterialPlayerPositionSetter : MonoBehaviour
    {
        private readonly int PlayerPosID = Shader.PropertyToID("_TargetPosition");

        [Inject] private Player _player;
        private Transform playerTrm;
        
        private void Awake()
        {
            if(_player != null)
                playerTrm = _player.transform;
        }

        void LateUpdate()
        {
            if (playerTrm != null)
            {
                Shader.SetGlobalVector(PlayerPosID, playerTrm.position);
            }
        }
    }
}