using UnityEngine;
using UnityEngine.UI;

public class atom_unitCard : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image unit;
    [SerializeField] private Image checkBox;

    public HeroData data;
    private bool alreadyHave = false;

    public bool AlredayHave
    {
        get
        {
            return alreadyHave;
        }

        set
        {
            alreadyHave = value;
            checkBox.gameObject.SetActive(alreadyHave);
        }
    }

    public void SetUnit(HeroData heroData)
    {
        this.data = heroData;
        
        var unitSprite = GameManager.Resource.LoadAsset<Sprite>(heroData.resourceKey);
        unit.sprite = unitSprite;

        background.color = Utility.GetColorByGrade((Grade)(heroData.heroId / 1000000 % 10));
        checkBox.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        checkBox.gameObject.SetActive(alreadyHave);
    }
}
