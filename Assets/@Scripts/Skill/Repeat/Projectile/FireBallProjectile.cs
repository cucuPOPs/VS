    using System.Collections;
    using UnityEngine;

    public class FireBallProjectile : Projectile
    {
        //충돌이 발생하면.
        //스킬에 의한 데미지를 주고,
        //파괴.
        private Vector3 _moveDir;
        public void SetInfo(Vector3 dir)
        {
            
            _moveDir = dir;
            
        }
        void Update()
        {
            Move();
            UpdateCell(this);
            CheckCollision();
        }

        void Move()
        {
            SetPos(GetPos() + _moveDir * (5f * Time.deltaTime));
        }

        void CheckCollision()
        {
            var monsters = Managers.Game.Grid.GatherObjects<UnitMonster>(CellIndex);
            foreach (var mon in monsters)
            {
                bool isCollision = IsCollision(mon);
                if (isCollision)
                {
                    OnDamaged(mon);
                    DestroyProjectile();
                    return;
                }
            }
            
        }
    }
