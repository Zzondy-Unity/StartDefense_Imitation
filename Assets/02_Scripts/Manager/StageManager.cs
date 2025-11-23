using UnityEngine;

public class StageManager : MonoBehaviour, IManager
{
    // 골드, 미네랄, 프로브의 정보를 가지고
    public int curGold { get; private set; }
    public int curMineral { get; private set; }
    public int curProbes { get; private set; }
    public int maxProbes { get; private set; } = 20;
    
    public void Init()
    {
        curGold = 0;
        curMineral = 0;
        curProbes = 0;
        
    }
    public bool CanUpgrade(HeroData heroData)
    {
        // 업글가능한 유닛인지 판단
        return false;
    }
    
    public Hero SpawnHero(Grade grade, TileNode tile)
    {
        var heroDatas = GameManager.Data.GetHerosByGrade(grade);
        var heroData = heroDatas[UnityEngine.Random.Range(0, heroDatas.Count)];
        var heroPrefab = GameManager.Resource.LoadAsset<Hero>(heroData.resourceKey);
        var hero = GameManager.Pool.GetFromPool(heroPrefab);
        hero.Init(heroData);
        tile.curHero = hero;
        return hero;
    }
}
