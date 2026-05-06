using System.Collections.Generic;
using System.Linq;
using Code.Modules;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Skills.Core
{
    [DefaultExecutionOrder(-10), Provide]
    public class SkillManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private GameEventChannelSO skillChannel;
        [Space]
        [SerializeField] private GameObject[] skillPrefabs;
        
        private Dictionary<SkillDataSO, Skill> _skillDict;
        
        private void Awake()
        {
            _skillDict = skillPrefabs.Select(obj => obj.GetComponentInChildren<Skill>())
                .ToDictionary(skill => skill.SkillData);
        }
        
        public Skill GetRandomSkill(ModuleOwner owner, Transform parent)
        {
            int rand = Random.Range(0, _skillDict.Count);
            Skill skill = _skillDict.ElementAt(rand).Value;
            
            Debug.Assert(skill != null, "Not Found Skill");
            
            Skill skillInstance = Instantiate(skill, parent);
            skillInstance.InitializeSkill(owner);
            
            return skillInstance;
        }
        
        public Skill GetSkill(SkillDataSO skillData, ModuleOwner owner, Transform parent)
        {
            Skill skill = _skillDict.GetValueOrDefault(skillData);
            
            Debug.Assert(skill != null, "Not Found Skill");
            
            Skill skillInstance = Instantiate(skill, parent);
            skillInstance.InitializeSkill(owner);
            
            return skillInstance;
        }
        
        public void AddSkill(Skill skill) => _skillDict.Add(skill.SkillData, skill);
        public void RemoveSkill(SkillDataSO skillData) => _skillDict.Remove(skillData);
    }
}