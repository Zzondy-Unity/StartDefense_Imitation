using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 영웅과 관련된 것들을 관리합니다.
/// </summary>
public class StageManager : MonoBehaviour, IManager
{
    [SerializeField] private Transform leftNexus;
    [SerializeField] private Transform rightNexus;
    [SerializeField] private Transform leftMineral;
    [SerializeField] private Transform rightMineral;
    [SerializeField] private Probe probePrefab;
    
    // 골드, 미네랄, 프로브의 정보를 가지고
    public int curGold { get; private set; }
    public int curMineral { get; private set; }
    public int curProbes { get; private set; }
    public int maxProbes { get; private set; }

    private int unitSummonCost = 20;
    private int probeCost = 50;
    private List<Probe> probeList = new List<Probe>();
    private bool isLeft = true;
    private bool doInit = true;
    public int normalEnhanceCost { get; private set; } = 20;
    public int hightEnhanceCost { get; private set; } = 50;

    public bool canSummon
    {
        get { return curGold >= unitSummonCost; }
    }

    private PlayerData playerData = new();
    
    private Dictionary<int, List<Hero>> spawnedHeros = new();

    public void Init()
    {
        playerData = GameManager.Data.GetPlayerData();

        isLeft = true;
        doInit = true;
        curGold = playerData.startGold;
        curMineral = playerData.startMineral;
        curProbes = playerData.startProbe;
        maxProbes = playerData.maxProbes;

        for (int i = 0; i < curProbes; i++)
        {
            SpawnProbe();
        }

        EventManager.Subscribe(GameEventType.GetGold, GetGold);
        EventManager.Subscribe(GameEventType.GetMineral, GetMineral);
        doInit = false;
    }
    
    public void OnGameEnd(bool isWin)
    {
        for (int i = 0; i < probeList.Count; i++)
        {
            probeList[i].OnEnd();
        }
        probeList.Clear();
        EventManager.UnSubscribe(GameEventType.GetGold, GetGold);
        EventManager.UnSubscribe(GameEventType.GetMineral, GetMineral);
    }

    #region <<<<<<<< 화폐 >>>>>>>>

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
            GameManager.UI.Refresh<UIFixTile>();
            GameManager.UI.Refresh<UIGameHUD>();
        }
    }

    #endregion

    #region <<<<<<<< 승급 및 초월 >>>>>>>>

    public bool CanUpgrade(int heroId)
    {
        // 업글가능한 유닛인지 판단
        if (spawnedHeros.ContainsKey(heroId))
        {
            return spawnedHeros[heroId].Count >= 2;
        }

        return false;
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
        int grade = (int)tileNode.curHero.heroGrade;
        grade++;
        Grade next = (Grade)grade;
        int upgradeId = GameManager.Data.GetHeroByGradeRandom(next);
        
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
    
    #endregion

    #region <<<<<<<< 영웅소환 및 제거 >>>>>>>>

    public Hero SpawnHero(Grade grade, TileNode tile)
    {
        var heroDatas = GameManager.Data.GetHerosByGrade(grade);
        var heroData = heroDatas[Random.Range(0, heroDatas.Count)];
        return SpawnHero(heroData.heroId, tile);
    }

    public Hero SpawnNormalHero(TileNode tile)
    {
        EventManager.Publish(GameEventType.GetGold, - unitSummonCost);
        return SpawnHero(Grade.common, tile);
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

    #endregion

    #region <<<<<<<< 탐사정 >>>>>>>>

    public bool SpawnProbe()
    {
        if (!CanSpawnProbe()) return false;
        // 파괴되는 오브젝트가 아니기때문에
        // 게임 내에서 씬이 끝날때까지 지속되기 때문에 풀링 사용x
        Probe newProbe = Instantiate(probePrefab, this.transform);
        if (isLeft)
        {
            newProbe.transform.position = leftNexus.position;
            newProbe.Init(leftMineral.position, leftNexus.position);
        }
        else
        {
            newProbe.transform.position = rightNexus.position;
            newProbe.Init(rightMineral.position, rightNexus.position);
        }
        isLeft = !isLeft;
        probeList.Add(newProbe);
        if (!doInit)
        {
            EventManager.Publish(GameEventType.GetGold, - probeCost);
        }
        return true;
    }

    private bool CanSpawnProbe()
    {
        if (doInit) return true;
        return curGold >= probeCost;
    }

    #endregion

    public void Enhance(Grade grade)
    {
        var list = GameManager.Data.GetHerosByGrade(grade);
        for (int i = 0; i < list.Count; i++)
        {
            if (spawnedHeros.TryGetValue(list[i].heroId, out var heros))
            {
                foreach (var hero in heros)
                {
                    hero.Enhance();
                }
            }
        }
        
    }
}