using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using TableData;
using UnityEngine;
using static Define;
using SkillData = TableData.SkillData;


public class TableDataManager : IManager
{
    private const string DATA_PATH = "Data";
    private bool _init = false;

    void IManager.Init()
    {
        if (_init) return;
        _init = true;
        LoadData();
    }

    void IManager.Clear()
    {
        
    }

    private void LoadData()
    {
        LoadChapterDataTable();
        LoadSkillDataTable();
    }
    

    #region CHAPTER_DATA
    private const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    public Dictionary<int, ChapterData> ChapterDic { get; private set; } = new Dictionary<int, ChapterData>();
    
    private void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");
    
        foreach (var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                ID = AsInt(data["ID"]),
                ChapterId = AsInt(data["ChapterId"]),
                TotalWave = AsInt(data["TotalWave"]),
                ChapterName = data["ChapterName"].ToString(),
            };

            ChapterDic.Add(chapterData.ID, chapterData);
        }
    }
    
    #endregion
    
    
    #region SKILL_DATA
    private const string SKILL_DATA_TABLE = "SkillDataTable";
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    
    private void LoadSkillDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{SKILL_DATA_TABLE}");
        
        List<SkillData> temp = new List<SkillData>();
        foreach (var data in parsedDataTable)
        {
            var skillData = new SkillData
            {
                ID = AsInt(data["ID"]),
                SkillId = AsInt(data["SkillId"]),
                Level = AsInt(data["Level"]),
                SkillName = AsString(data["SkillName"]),
                SkillType = AsEnum<SkillType>(data["SkillType"]),
                CoolTime = AsFloat(data["CoolTime"]),
                Damage = AsInt(data["Damage"]),
                NumProjectiles = AsInt(data["NumProjectiles"]),
            };

            temp.Add(skillData);
        }

        SkillDic = temp.GroupBy(rawSkill => rawSkill.SkillId)
            .ToDictionary(group => group.Key, group => new Data.SkillData
            {
                skillID = group.Key,
                skillName = group.First().SkillName,
                skillType = group.First().SkillType,
                maxLevel = group.Max(item => item.Level),
                damage = group.OrderBy(item => item.Level).Select(x => x.Damage).ToArray(),
                numProjectiles = group.OrderBy(item => item.Level).Select(x => x.NumProjectiles).ToArray(),
                cooltime = group.OrderBy(item => item.Level).Select(x => x.CoolTime).ToArray(),

            });

    }
    
    #endregion
    
    


    
    #region HELPER_METHODS
    private T AsEnum<T>(object value) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), AsString(value));
    }

    private int AsInt(object value)
    {
        return Convert.ToInt32(value);
    }
    
    private float AsFloat(object value)
    {
        return Convert.ToSingle(value);
    }

    private double AsDouble(object value)
    {
        return Convert.ToDouble(value);
    }
    private string AsString(object value)
    {
        return Convert.ToString(value);
    }
    #endregion
}