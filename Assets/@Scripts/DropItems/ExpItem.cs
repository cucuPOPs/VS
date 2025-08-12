using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ExpItem : DropItem
{
    public override Define.ObjectType ObjectType => Define.ObjectType.Exp;
    

    public override void CompleteGetItem()
    {
        //todo: exp 획득로직.
        base.CompleteGetItem();
    }
}