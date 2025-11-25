using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mole_Enhance : MonoBehaviour
{
    [SerializeField] private cell_enhance normal_rare;
    [SerializeField] private cell_enhance epic_legendary;
    [SerializeField] private cell_enhance nothing;

    private readonly string n_rImagePath = "Images/n_rImage";
    private readonly string e_ImagePath = "Images/e_lImage";

    private GameSceneManager manager => GameManager.Scene.GetSceneManager<GameSceneManager>();
    public void Init()
    {
        Sprite n_r = GameManager.Resource.LoadAsset<Sprite>(n_rImagePath);
        Sprite e_l = GameManager.Resource.LoadAsset<Sprite>(e_ImagePath);
        normal_rare.Init(n_r,manager.Stage.normalEnhanceCost , 0, OnClick);
        epic_legendary.Init(e_l,manager.Stage.hightEnhanceCost , 1, OnClick);
    }

    private void OnClick(int idx)
    {
        if (manager == null) return;
        
        if (idx == 0)
        {
            EventManager.Publish(GameEventType.GetMineral, -manager.Stage.normalEnhanceCost);
            manager.Stage.Enhance(Grade.common);
            manager.Stage.Enhance(Grade.normal);
            manager.Stage.Enhance(Grade.rare);
        }
        else if (idx == 1)
        {
            EventManager.Publish(GameEventType.GetMineral, -manager.Stage.hightEnhanceCost);
            manager.Stage.Enhance(Grade.epic);
            manager.Stage.Enhance(Grade.legendary);
        }
        else
        {
            // 오류
            Logger.WarningLog($"[Mole_Enhance] OnClick idx: {idx}");
        }
    }

    public void Refresh()
    {
        if (manager == null) return;
        int curMineral = manager.Stage.curMineral;
        int normalEnhanceCost = manager.Stage.normalEnhanceCost;
        int hightEnhanceCost = manager.Stage.hightEnhanceCost;
        bool canEnhanceNormal = normalEnhanceCost <= curMineral;
        bool canEnhanceHight = hightEnhanceCost <= curMineral;
        
        normal_rare.Refresh(canEnhanceNormal);
        epic_legendary.Refresh(canEnhanceHight);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
