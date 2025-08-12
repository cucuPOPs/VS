public class GoldItem : DropItem
{
    public override Define.ObjectType ObjectType => Define.ObjectType.Gold;
    
    public override void CompleteGetItem()
    {
        base.CompleteGetItem();
        //todo.
        //플레이어 골드증가.
    }
}