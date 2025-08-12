using System.Collections;

using UnityEngine;

public class FireBallSkill : RepeatSkill
{
    public override Define.SkillType SkillType => Define.SkillType.FireBall;

    //Projectile을 생성.
    //projectile에 스킬정보를 넘김.
    //projectile은 몬스터랑 충돌감지해서. onwer가 너를 공격했다를 처리.
    protected override void DoSkill()
    {
        int numProjectiles = Data.numProjectiles[CurLevel - 1];
        if (numProjectiles > 0)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                float angle = i * Mathf.PI * 2f / numProjectiles;
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);
                var proj = GenerateProjectile<FireBallProjectile>();
                proj.SetInfo(new Vector3(x, y));
            }
        }
    }
}