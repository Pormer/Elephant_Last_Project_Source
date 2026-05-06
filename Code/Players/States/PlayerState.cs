using Code.Entities;
using Code.Entities.FSM;
using Code.Entities.Modules;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        protected EntityMovement _movement;
        
        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            if (_player == null)
            {
                Debug.LogError("state variable: player is null");
                return;
            }
            
            _movement = _player.GetModule<EntityMovement>();
            Debug.Assert(_movement != null, "state variable: movement is null");
        }
    }
}