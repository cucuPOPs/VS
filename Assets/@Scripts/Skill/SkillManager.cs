using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private UnitBase owner;
    private List<SkillBase> AllSkills { get; } = new List<SkillBase>();
    private List<RepeatSkill> RepeatSkills { get; } = new List<RepeatSkill>();
    private List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();

    public void Init(UnitBase owner)
    {
        this.owner = owner;
        AllSkills.Clear();
        RepeatSkills.Clear();
        SequenceSkills.Clear();
    }

    public void AddSkill(int skillID, int level = 1)
    {
        Data.SkillData data = Managers.Table.SkillDic[skillID];

        string className = $"{data.skillType}Skill";
        var skill = gameObject.AddComponent(Type.GetType(className)) as SkillBase;
        if (skill == null)
        {
            Debug.LogWarning("addskill failed");
            return;
        }

        skill.Init();
        skill.SetInfo(owner, skillID, level);
        AllSkills.Add(skill);

        switch (skill)
        {
            case SequenceSkill sequenceSkill:
                SequenceSkills.Add(sequenceSkill);
                break;
            case RepeatSkill repeatSkill:
                repeatSkill.Activate();
                RepeatSkills.Add(repeatSkill);
                break;
        }
    }
    
    
    int _sequenceIndex = 0;

    public void StartNextSequenceSkill()
    {
        if (_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;
		
        SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    void OnFinishedSequenceSkill()
    {
        _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    bool _stopped = false;

    public void StopSkills()
    {
        _stopped = true;

        foreach (var skill in RepeatSkills)
        {
            skill.StopAllCoroutines();
        }
    }
}
