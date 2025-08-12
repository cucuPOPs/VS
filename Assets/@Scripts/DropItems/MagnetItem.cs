public class MagnetItem : DropItem
{
    public override Define.ObjectType ObjectType => Define.ObjectType.Magnet;

    
    public override void CompleteGetItem()
    {
        base.CompleteGetItem();
        Managers.Object.CollectAllItems();
    }
}