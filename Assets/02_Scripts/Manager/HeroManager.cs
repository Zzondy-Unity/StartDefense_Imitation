using System;
using UnityEngine;

public class HeroManager : MonoBehaviour, IManager
{
    // 영웅의 소환 담당
    // 영웅의 개수를 알고있고
    // 승급가능하면 승급도와줌
    
    public void Init()
    {
        
    }

    public Hero SpawnHero(Grade grade)
    {
        var heroDatas = GameManager.Data.GetHerosByGrade(grade);
        var heroData = heroDatas[UnityEngine.Random.Range(0, heroDatas.Count)];
        var heroPrefab = GameManager.Resource.LoadAsset<Hero>(heroData.resourceKey);
        var hero = GameManager.Pool.GetFromPool(heroPrefab);
        hero.Init(heroData);
        return hero;
    }
}
