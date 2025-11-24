using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour, IManager
{
    // 골드, 미네랄, 프로브의 정보를 가지고
    public int curGold { get; private set; }
    public int curMineral { get; private set; }
    public int curProbes { get; private set; }
    public int maxProbes { get; private set; }

    private int unitSummonCost = 20;

    public bool canSummon
    {
        get { return curGold >= unitSummonCost; }
    }

    private PlayerData playerData = new();
    
    private Dictionary<int, List<Hero>> spawnedHeros = new();

    public void Init()
    {
        playerData = GameManager.Data.GetPlayerData();
        
        curGold = playerData.startGold;
        curMineral = playerData.startMineral;
        curProbes = playerData.startProbe;
        maxProbes = playerData.maxProbes;

        EventManager.Subscribe(GameEventType.GetGold, GetGold);
        EventManager.Subscribe(GameEventType.GetMineral, GetMineral);
    }

    public void OnGameEnd(bool isWin)
    {
        EventManager.UnSubscribe(GameEventType.GetGold, GetGold);
        EventManager.UnSubscribe(GameEventType.GetMineral, GetMineral);
    }

    public void GetGold(object org)
    {
        if (org is int amount)
        {
            curGold += amount;
            GameManager.UI.Refresh<UISummon>();
            GameManager.UI.Refresh<UIGameHUD>();
        }
    }

    public void GetMineral(object org)
    {
        if (org is int amount)
        {
            curMineral += amount;
            GameManager.UI.Refresh<UIGameHUD>();
        }
    }

    public bool CanUpgrade(int heroId)
    {
        // 업글가능한 유닛인지 판단
        if (spawnedHeros.ContainsKey(heroId))
        {
            return spawnedHeros[heroId].Count >= 2;
        }

        return false;
    }

    public Hero SpawnHero(Grade grade, TileNode tile)
    {
        var heroDatas = GameManager.Data.GetHerosByGrade(grade);
        var heroData = heroDatas[Random.Range(0, heroDatas.Count)];
        return SpawnHero(heroData.heroId, tile);
    }

    public Hero SpawnHero(int id, TileNode tile)
    {
        var heroData = GameManager.Data.GetHeroByID(id);
        var heroPrefab = GameManager.Resource.LoadAsset<Hero>(heroData.resourceKey);
        var hero = GameManager.Pool.GetFromPool(heroPrefab);
        hero.Init(heroData, tile);
        tile.curHero = hero;

        if (spawnedHeros.ContainsKey(hero.id))
        {
            spawnedHeros[hero.id].Add(hero);
        }
        else
        {
            var list = new List<Hero>();
            spawnedHeros.Add(hero.id, list);
            spawnedHeros[hero.id].Add(hero);
        }

        hero.transform.position = tile.worldPos;
        GetGold(- unitSummonCost);
        GameManager.UI.RefreshAll();
        return hero;
    }

    public void RemoveHero(Hero hero)
    {
        RemoveHero(hero.id);
    }

    public void RemoveHero(int id)
    {
        if (spawnedHeros.ContainsKey(id) && spawnedHeros[id].Count > 0)
        {
            var removal = spawnedHeros[id][0];
            spawnedHeros[id].Remove(removal);
            GameManager.Pool.ReturnToPool(removal);
        }
        else
        {
            Logger.WarningLog($"[StageManager] 데이터에 없는 영웅을 제거하고자 시도하였습니다. 영웅 : {id}");
        }
    }

    public void UpgradeHero(TileNode tileNode)
    {
        if (tileNode == null)
        {
            Logger.WarningLog($"[StageManager] 영웅 승급을 시도하였으나 해당 타일정보가 들어오지 않았습니다. {tileNode}");
            return;
        }

        if (tileNode.curHero == null)
        {
            Logger.WarningLog($"[StageManager] 영웅 승급을 시도하였으나 해당 타일에 영웅이 존재하지 않습니다. {tileNode}");
            return;
        }
        if (!spawnedHeros.ContainsKey(tileNode.curHero.id))
        {
            Logger.WarningLog($"[StageManager] 영웅 승급을 시도하였으나 해당 영웅 인스턴스리스트가 존재하지 않습니다.{tileNode.curHero.id}");
            return;
        }

        if (!CanUpgrade(tileNode.curHero.id))
        {
            Logger.WarningLog($"[StageManager] 업그레이드 할 수 없습니다.{tileNode.curHero.id}");
            return;
        }
        
        // 해당영웅 2마리 제거
        int id = tileNode.curHero.id;
        int upgradeId = tileNode.curHero.data.combinationHero;
        RemoveHero(tileNode.curHero);
        RemoveHero(id);
        // 새로운 다음 등급 영웅 해당 타일에 소환
        SpawnHero(upgradeId, tileNode);
    }

    public void Trescend(TileNode tile, int id)
    {
        //해당영웅 제거
        RemoveHero(tile.curHero);
        SpawnHero(id, tile);
    }
}