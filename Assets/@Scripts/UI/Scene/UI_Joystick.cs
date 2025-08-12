using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : UI_Scene
{
    [SerializeField] private float _deadZone = 0.1f;
    [SerializeField] private float _fadeDuration = 0.5f;

    private float _radius;
    private Vector2 _input = Vector2.zero;

    enum Images
    {
        JoystickBG,
        Handler
    }

    enum RTs
    {
        JoystickBG,
        Handler
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindRT(typeof(RTs));

        _radius = GetRT((int)RTs.JoystickBG).sizeDelta.y * 0.5f;

        // 드래그 바인딩
        gameObject.BindEvent(action: OnPointerDown, type: Define.UIEvent.PointerDown);
        gameObject.BindEvent(action: OnPointerUp, type: Define.UIEvent.PointerUp);
        gameObject.BindEvent(dragAction: OnDrag, type: Define.UIEvent.Drag);

        SetActiveJoystick(false);
        return true;
    }

    private void OnPointerDown()
    {
        SetActiveJoystick(true);
        GetRT((int)RTs.JoystickBG).anchoredPosition = Input.mousePosition;
        ResetHandlerPosition();
    }

    private void OnDrag(PointerEventData eventData)
    {
        Vector2 curPos = eventData.position;
        Vector2 center = GetRT((int)RTs.JoystickBG).anchoredPosition;

        _input = (curPos - center) / _radius;

        if (_input.sqrMagnitude > 1f)
        {
            _input = _input.normalized;
        }

        GetRT((int)RTs.Handler).anchoredPosition = _input * _radius;

        UpdateMoveDirection();
    }

    public void OnPointerUp()
    {
        ResetInput();
        SetActiveJoystick(false);
    }

    private void UpdateMoveDirection()
    {
        Vector2 moveDirection = _input.magnitude < _deadZone ? Vector2.zero : _input;
        Managers.Game.MoveDir = moveDirection;
    }

    private void ResetInput()
    {
        _input = Vector2.zero;
        Managers.Game.MoveDir = Vector2.zero;
    }

    private void ResetHandlerPosition()
    {
        GetRT((int)RTs.Handler).anchoredPosition = Vector2.zero;
    }

    private void SetActiveJoystick(bool isActive)
    {
        float targetAlpha = isActive ? 1f : 0f;
        GetImage((int)Images.Handler).DOFade(targetAlpha, _fadeDuration);
        GetImage((int)Images.JoystickBG).DOFade(targetAlpha, _fadeDuration);
    }

}