using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cell_enhance : MonoBehaviour
{
    [SerializeField] private atom_buy enhance;
    [SerializeField] private Image image;
    private int index;
    private Action<int> onClick;

    public void Init(Sprite eL, int cost,  int i, Action<int> _onClick)
    {
        index = i;
        image.sprite = eL;
        onClick = _onClick;
        enhance.SetAtom(MoneyType.mineral, cost, OnClick, OnAfterClick);
    }

    public void Refresh(bool canBuy)
    {
        enhance.Refresh(canBuy);
    }

    private void OnAfterClick()
    {
        var hud = GameManager.UI.Get<UIGameHUD>(out bool isOpen);
        hud.HideEnhance();
    }

    private void OnClick()
    {
        onClick?.Invoke(index);
    }
}
