using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_Supply : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI supplyTxt;
    [SerializeField] private Transform middlePart;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderAmount;
    [SerializeField] private atom_unitCard unitCardPrefab;

    private List<atom_unitCard> unitCards = new List<atom_unitCard>();

    public void Set(List<HeroData> datas)
    {
        supplyTxt.text = "SUPPLY";
        int count = datas.Count;
        for (int i = 0; i < count; i++)
        {
            var card = Instantiate(unitCardPrefab, middlePart);
            card.SetUnit(datas[i]);
            unitCards.Add(card);
        }

        slider.maxValue = count;
        slider.value = 0;
        SetText();
    }

    public void Refresh()
    {
        int count = 0;
        for (int i = 0; i < unitCards.Count; i++)
        {
            var unitData = unitCards[i].data;
            unitCards[i].AlredayHave = CheckAlreadyHas();
            if (unitCards[i].AlredayHave)
            {
                count++;
            }
        }
        slider.value = count;
        SetText();
    }

    private void SetText()
    {
        sliderAmount.text = $"( {slider.value} / {slider.maxValue} )";
    }

    //TODO:son:check - 빼두고 진행 예정
    private bool CheckAlreadyHas()
    {
        return false;
    }
}