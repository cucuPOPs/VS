public class BombItem : DropItem
{
    public override Define.ObjectType ObjectType => Define.ObjectType.Bomb;
    public override void CompleteGetItem()
    {
        base.CompleteGetItem();
        //todo.
        //모든 몬스터를 Kill.
        //보스는?
    }
}