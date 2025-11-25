using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_buy : UIPopUp
{
    [SerializeField] private TextMeshProUGUI textTMP;
    [SerializeField] private Button button;
    [SerializeField] private atom_ImgTmp costIT;

    public void SetAtom(MoneyType moneyType, int cost, UnitPopupMode popupMode, Action onClick, Action afterClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
            {
                onClick?.Invoke();
                afterClick?.Invoke();
            }
        );
        
        Sprite moneySprite = Utility.GetMoneyImage(moneyType);
        costIT.image.sprite = moneySprite;
        textTMP.text = popupMode.ToString();
        button.image.color = Utility.GetColorByPopupMode(popupMode);
        
        costIT.text = cost.ToString();
    }

    public void SetAtom(MoneyType moneyType, int cost, Action onClick,Action afterClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
            {
                onClick?.Invoke();
                afterClick?.Invoke();
            }
        );
        
        Sprite moneySprite = Utility.GetMoneyImage(moneyType);
        costIT.image.sprite = moneySprite;
        costIT.text = cost.ToString();
    }

    public void Refresh(bool canBuy)
    {
        costIT.textColor = canBuy ? Color.green : Color.red;
        button.interactable = canBuy;
    }
    
}