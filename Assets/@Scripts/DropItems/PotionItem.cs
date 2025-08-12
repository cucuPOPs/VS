public class PotionItem : DropItem
{
    public override Define.ObjectType ObjectType => Define.ObjectType.Heal;
    public override void CompleteGetItem()
    {
        //temp
        float healAmount = 30;

        Managers.Object.Player.Healing(healAmount);
        Managers.Object.Despawn(this);

    }
}