using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_buy : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI textTMP;
   [SerializeField] private Button button;
   [SerializeField] private TextMeshProUGUI costTMP;
   [SerializeField] private Image costImage;

   public void SetAtom(MoneyType moneyType, int cost, UnitPopupMode popupMode, Action onClick)
   {
      button.onClick.RemoveAllListeners();
      button.onClick.AddListener(() => onClick?.Invoke());
      
      Sprite moneySprite = Utility.GetMoneyImage(moneyType);
      costImage.sprite = moneySprite;
      textTMP.text = popupMode.ToString();
      button.image.color = Utility.GetColorByPopupMode(popupMode);

      // TODO :: 구매가능여부에 따라 글자색 변경
      costTMP.text = cost.ToString();
   }
}
