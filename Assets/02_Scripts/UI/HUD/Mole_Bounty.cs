using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Mole_Bounty : MonoBehaviour
{
    [SerializeField] private ScrollRect bountyScrollRect;
    [SerializeField] private cell_bounty cellBounty;
    [SerializeField] private Transform scrollContent;
    private List<cell_bounty> bountyCells = new List<cell_bounty>();
    
    public void Init()
    {
        List<int> list = GameManager.Data.GetBountyMonster();
        for (int i = 0; i < list.Count; i++)
        {
            var cell = Instantiate(cellBounty, scrollContent);
            
            MonsterData data = GameManager.Data.GetMonsterByID(list[i]);
            Sprite monsterSprite = GameManager.Resource.LoadAsset<Sprite>(data.spriteKey);
            cell.Init(monsterSprite, data.monsterDesc, list[i], OnClick);
            
            bountyCells.Add(cell);
        }
        Hide();
    }

    private void OnClick(int index)
    {
        var manager = GameManager.Scene.curSceneManager as GameSceneManager;
        if (manager != null)
        {
            manager.Wave.SpawnMonster(index);
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        
    }
}
