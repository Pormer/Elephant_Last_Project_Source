using Code.Skills;
using Code.Skills.Core;
using Code.Souls.Core;

namespace EventSystem
{
    public static class SkillEvents
    {
        public static readonly SoulActiveEvent SoulActiveEvent = new SoulActiveEvent();
        public static readonly PlayerSkillEquipEvent PlayerSkillEquipEvent = new PlayerSkillEquipEvent();
        public static readonly PlayerSoulEquipEvent PlayerSoulEquipEvent = new PlayerSoulEquipEvent();
        public static readonly PlayerSoulChangeEvent PlayerSoulChangeEvent = new PlayerSoulChangeEvent();
        public static readonly SkillLockEvent SkillLockEvent = new SkillLockEvent();
        public static readonly PlayerRemoveEquipSoulEvent PlayerRemoveEquipSoulEvent = new PlayerRemoveEquipSoulEvent();
    }
    
    //인벤토리에 소울 추가, 제거 요청
    public class SoulActiveEvent : GameEvent
    {
        public SoulDataSO targetSoul;
        public bool isActive;

        public SoulActiveEvent Initializer(SoulDataSO targetSoul, bool isActive)
        {
            this.targetSoul = targetSoul;
            this.isActive = isActive;
            return this;
        }
    }

    //현재 슬롯에 스킬 장착 요청
    public class PlayerSkillEquipEvent : GameEvent
    {
        public SkillKeyType targetType;
        public SkillDataSO targetSkillData;
        
        public PlayerSkillEquipEvent Initializer(SkillKeyType type, SkillDataSO skillData)
        {
            targetType = type;
            targetSkillData = skillData;
            return this;
        }
    }
    
    //슬롯에 새로운 소울을 장착 요청
    public class PlayerSoulEquipEvent : GameEvent
    {
        public SoulType targetType;
        public SoulDataSO targetSoul;
        
        public PlayerSoulEquipEvent Initializer(SoulType type, SoulDataSO ability)
        {
            targetType = type;
            targetSoul = ability;
            return this;
        }
    }

    //현재 사용하는 슬롯이 다른 슬롯의 소울로 변경되었음을 알림
    public class PlayerSoulChangeEvent : GameEvent
    {
        public SoulDataSO targetSoul;
        
        public PlayerSoulChangeEvent Initializer(SoulDataSO soul)
        {
            targetSoul = soul;
            return this;
        }
    }

    //해당 슬롯 삭제
    public class PlayerRemoveEquipSoulEvent : GameEvent
    {
        public SoulType targetType;
        public SoulDataSO targetSoul;
        
        public PlayerRemoveEquipSoulEvent Initializer(SoulType type, SoulDataSO soul)
        {
            targetType = type;
            targetSoul = soul;
            return this;
        }
    }

    //해당 스킬 사용 가능 여부 변경 
    public class SkillLockEvent : GameEvent
    {
        public SkillDataSO targetSkill;
        public SkillKeyType targetKey;
        public bool isLock;

        public SkillLockEvent Initializer(SkillDataSO targetSkill, SkillKeyType targetKey, bool isLock)
        {
            this.targetSkill = targetSkill;
            this.targetKey = targetKey;
            this.isLock = isLock;
            return this;
        }
    }
}