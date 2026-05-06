using Code.Entities;
using Code.Players.Modules;
using Code.Players.States.PlayerSkillStates;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerCanAttackState : PlayerCanUseSkillState
    {
        protected PlayerAttackComponent _attackCompo;
        private PlayerSoulManagement _soulManagement;

        private bool _canSlotChanged;

        public PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _soulManagement = entity.GetModule<PlayerSoulManagement>();
            _attackCompo = entity.GetModule<PlayerAttackComponent>();

            Debug.Assert(_attackCompo != null, "state variable: attack component is null");
            Debug.Assert(_soulManagement != null, "state variable: ability management is null");
        }

        public override void Enter()
        {
            base.Enter();

            _canSlotChanged = true;
            if (_movement.IsGrounded) _attackCompo.SetCanAttack(true);

            _player.PlayerInput.OnAttackPressed += HandleAttackPressed;
            _player.PlayerInput.OnSlotChangePressed += HandleSlotChangePressed;
            
            _soulManagement.OnActiveSoulChangedEvent.AddListener(HandleEnablePassiveSkill);
        }

        protected virtual void HandleAttackPressed()
        {
            _preInputModule.AddToBuffer("ATTACK", 1);
        }

        private async void HandleSlotChangePressed()
        {
            if (!_canSlotChanged) return;

            _canSlotChanged = false;

            //Slot change section
            if (_soulManagement.CanChangeSlot())
            {
                UseSkill(SkillKeyType.SWITCH_PASSIVE_SKILL);
                
                CancelPassiveSkill();
                _soulManagement.ChangeSlot();
            }

            await Awaitable.WaitForSecondsAsync(0.5f, _entity.destroyCancellationToken);
            _canSlotChanged = true;
        }
        
        private void HandleEnablePassiveSkill() => UseSkill(SkillKeyType.PASSIVE_SKILL);

        protected override void UseSkill(SkillKeyType keyType, bool isPressed = true)
        {
            if (!isPressed) return;

            var skill = _skillManagement.GetSkill(keyType);
            if (skill == null || !_skillManagement.CanUseSkill(skill))
            {
                //스킬 없음 Feedback보내기
                if (skill != null)
                {
                    Debug.Log($"스킬이 쿨타임중이거나 사용중이거나 마나가 부족합니다. : {skill.name}");
                }
                else
                {
                    Debug.Log($"장착하고 있는 스킬 없음 {keyType}");
                }

                return;
            }

            _skillManagement.SetCurrentSkill(keyType);

            if (!skill.SkillData.useChangeState)
            {
                _skillManagement.TryUseSkill(skill);
                return;
            }

            if (skill is IChargeable { IsCharging: false } chargeable)
            {
                chargeable.StartCharging();
                _player.ChangeState("CHARGING");
            }
            else
                _player.ChangeState(skill.SkillData.skillStateName);
        }
        
        private void CancelPassiveSkill()
        {
            var skill = _skillManagement.GetSkill(SkillKeyType.PASSIVE_SKILL);
            if (skill == null) return;
            skill.CancelSkill();
        }

        public override void Exit()
        {
            _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
            _player.PlayerInput.OnSlotChangePressed -= HandleSlotChangePressed;
            _soulManagement.OnActiveSoulChangedEvent.RemoveListener(HandleEnablePassiveSkill);
            
            base.Exit();
        }
    }
}