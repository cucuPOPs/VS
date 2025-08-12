using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIManager :IManager
{
    int _order = 10;
    int _toastOrder = 500;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    Stack<UI_Toast> _toastStack = new Stack<UI_Toast>();
    UI_Scene _sceneUI = null;

    public UI_Scene SceneUI { get { return _sceneUI; } }
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0, bool isToast = false)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        if (canvas == null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }

        CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1080, 1920);
        }

        go.GetOrAddComponent<GraphicRaycaster>();

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = sortOrder;
        }

        if (isToast)
        {
            _toastOrder++;
            canvas.sortingOrder = _toastOrder;
        }

    }

    public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"{name}", parent, pooling);
        go.transform.SetParent(parent);
        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.SetActive(false);
        go.transform.SetParent(Root.transform);
        go.SetActive(true);

        sceneUI.Init();
        sceneUI.SetInfo();
        sceneUI.ShowUI();
        return sceneUI;
    }

    #region PopupUI
    public T OpenPopupUI<T>(BaseUIData data = null, string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        
        //get.
        GameObject go = Managers.Resource.Instantiate($"{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);
        go.SetActive(false);
        go.transform.SetParent(Root.transform);
        go.SetActive(true);
        
        //set.
        popup.Init();
        popup.SetInfo(data);
        
        //show.
        popup.ShowUI();


        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (popup != _popupStack.Peek())
        {
            Debug.LogError("close popup failed!");
            return;
        }
        CloseFrontPopupUI();
    }

    public void CloseFrontPopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            CloseFrontPopupUI();
    }


    public int GetPopupCount()
    {
        return _popupStack.Count;
    }

    #endregion
    
    #region ToastUI
    public T ShowToast<T>(BaseUIData data = null,string name=null) where T : UI_Toast
    {
        
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        
        GameObject go = Managers.Resource.Instantiate($"{name}", pooling: true);
        T popup = Util.GetOrAddComponent<T>(go);
        _toastStack.Push(popup);
        go.SetActive(false);
        go.transform.SetParent(Root.transform);
        go.SetActive(true);
        
        popup.Init();
        popup.SetInfo(data);
        
        popup.ShowUI();
        return popup;
    }
    
    public void CloseToastUI(UI_Toast toast)
    {
        if (toast != _toastStack.Peek())
        {
            Debug.LogError("close toast failed!");
            return;
        }
        CloseToastUI();
    }
    public void CloseToastUI()
    {
        if (_toastStack.Count == 0)
            return;

        UI_Toast toast = _toastStack.Pop();
        Managers.Resource.Destroy(toast.gameObject);
        _toastOrder--;
    }
    public void CloseAllToastUI()
    {
        while (_toastStack.Count > 0)
            CloseToastUI();
    }
    #endregion

    void IManager.Init()
    {
    }

    void IManager.Clear()
    {
        CloseAllPopupUI();
        CloseAllToastUI();
    }

}
