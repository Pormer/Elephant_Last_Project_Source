using Code.Entities;
using Code.Entities.Modules;
using Code.Interactable;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerInteractState : PlayerState
    {
        private EntityRenderer _renderer;
        private Interactor _interactor;

        public PlayerInteractState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _interactor = entity.GetModule<Interactor>();
            _renderer = entity.GetModule<EntityRenderer>();

            Debug.Assert(_interactor != null, $"collector is not found: {entity.name}");
            Debug.Assert(_renderer != null, "state variable: renderer is null");
        }

        public override void Enter()
        {
            if (!_interactor.Interact(out var curInteractable))
            {
                _player.ChangeState("IDLE");
                return;
            }

            base.Enter();

            _renderer.SetFacingRight(curInteractable.Transform.position.x > _player.transform.position.x);
            _movement.SetCanManualMove(false);
        }

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _movement.SetCanManualMove(true);
            base.Exit();
        }
    }
}