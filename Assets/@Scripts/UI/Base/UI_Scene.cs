using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Scene : UI_Base
{
   public override bool Init()
    {
        if (base.Init() == false)
            return false;

        var obj = FindAnyObjectByType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("EventSystem").name = "@EventSystem";
        
        Managers.UI.SetCanvas(gameObject, false);
        return true;
    }
}
