using UnityEngine;

namespace TableData
{
    public class ChapterData
    {
        public int ID;
        public int ChapterId;
        public int TotalWave;
        public string ChapterName;
    }
    
    
    public class SkillData
    {
        public int ID;
        public int SkillId;
        public int Level;
        public string SkillName;
        public Define.SkillType SkillType;
        public int Damage;
        public float CoolTime;
        public int NumProjectiles;
    }
}

//파싱후 데이터.
namespace Data
{
    public class SkillData
    {
        public int skillID;
        public string skillName;
        public Define.SkillType skillType;
        public int maxLevel;

        //레벨별 데이터.
        public int[] damage;
        public float[] cooltime;
        public int[] numProjectiles;
    }
}