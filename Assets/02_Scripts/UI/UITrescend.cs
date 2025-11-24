using System;
using System.Collections.Generic;
using UnityEngine;

public class UITrescend : UIPopUp
{
    [SerializeField] private atom_trescendInfo trescendPrefab;
    [SerializeField] private Transform prefabParent;
    private TileNode _tile;
    private List<atom_trescendInfo> trescends = new();
    
    public override void Opened(object[] param)
    {
        base.Opened(param);

        ResetList();
        
        if (param[0] is TileNode node)
        {
            _tile = node;
            int[] list = node.curHero.data.transcendList;
            for (int i = 0; i < list.Length; i++)
            {
                var instance = Instantiate(trescendPrefab, prefabParent);
                bool can = false;
                int[] own = GameManager.Data.GetPlayerData().ownedTrescend;
                for (int j = 0; j < own.Length; j++)
                {
                    if (list[i] == own[j])
                    {
                        can = true;
                        break;
                    }
                }

                HeroData data = GameManager.Data.GetHeroByID(list[i]);
                instance.Set(data, can, OnClick);
                trescends.Add(instance);
            }
        }
    }

    private void OnClick(int id)
    {
        if (_tile == null) return;
        gameSceneManager.Stage.Trescend(_tile, id);
        _tile = null;
        ResetList();
        Hide<UITrescend>();
    }

    public override void Refresh()
    {
        
    }

    private void ResetList()
    {
        for (int i = 0; i < trescends.Count; i++)
        {
            Destroy(trescends[i].gameObject);
        }
        trescends.Clear();
    }
}

