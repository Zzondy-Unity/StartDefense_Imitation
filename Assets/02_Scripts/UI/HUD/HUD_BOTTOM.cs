using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD_BOTTOM : MonoBehaviour
{
    [SerializeField] private atom_ImgTmp gold;

    [SerializeField] private atom_ImgTmp mineral;
    [SerializeField] private Button bountyBtn;
    [SerializeField] private Button probeBtn;

    private UIGameHUD _hud;
    
    public void Init(UIGameHUD hud)
    {
        _hud = hud;
        
        gold.image.sprite = Utility.GetMoneyImage(MoneyType.gold);
        mineral.image.sprite = Utility.GetMoneyImage(MoneyType.mineral);
        
        bountyBtn.onClick.RemoveAllListeners();
        probeBtn.onClick.RemoveAllListeners();
        
        bountyBtn.onClick.AddListener(OnBountyBtn);
        probeBtn.onClick.AddListener(OnProbeBtn);

        Refresh();
    }

    private void OnProbeBtn()
    {
        var manager = GameManager.Scene.curSceneManager as GameSceneManager;
        if (manager != null)
        {
            if (!manager.Stage.SpawnProbe())
            {
                Logger.Log($"[HUD_BOTTOM] probe 소환에 실패하였습니다. 돈이 부족합니다.");
            }
            _hud.Refresh();
        }
    }

    private void OnBountyBtn()
    {
        _hud.ShowBounty();
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
