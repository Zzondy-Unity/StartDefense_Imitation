using System;
using UnityEngine;

public class HUD_BOTTOM : MonoBehaviour
{
    [SerializeField] private atom_ImgTmp gold;

    [SerializeField] private atom_ImgTmp mineral;
    
    public void Init()
    {
        gold.image.sprite = Utility.GetMoneyImage(MoneyType.gold);
        mineral.image.sprite = Utility.GetMoneyImage(MoneyType.mineral);

        Refresh();
    }

    public void Refresh()
    {
        var manager = GameManager.Scene.curSceneManager as GameSceneManager;
        if (manager != null)
        {
            gold.text = manager.Stage.curGold.ToString();
            mineral.text = manager.Stage.curMineral.ToString();
        }
    }
}
