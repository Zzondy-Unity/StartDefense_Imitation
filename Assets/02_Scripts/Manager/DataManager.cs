using System.Collections.Generic;
using UnityEngine;

public class DataManager : IManager
{
    private readonly Dictionary<int, MonsterData> monsterDataBase = new();          // id에 해당하는 몬스터정보
    private readonly Dictionary<int, HeroData> heroDataBase = new();                // id에 해당하는 영웅정보
    private readonly Dictionary<int, List<MonsterWaveData>> waveDataBase = new();   // round별 웨이브 정보

    // 등급별로 모아두고 사용할 때
    private readonly Dictionary<Grade, List<int>> gradeHeroData = new Dictionary<Grade, List<int>>()
    {
        { Grade.common, new List<int>() },
        { Grade.normal, new List<int>() },
        { Grade.rare, new List<int>() },
        { Grade.epic, new List<int>() },
        { Grade.legendary, new List<int>() },
    };

    private readonly string jsonPath = "JsonFiles/";
    private readonly string heroFileName = "Hero";
    private readonly string monsterFileName = "Monster";
    private readonly string waveFileName = "MonsterWave";
    
    public void Init()
    {
        HeroSheetParse();
        MonsterSheetParse();
        WaveSheetParse();
    }

    private void HeroSheetParse()
    {
        var json = GameManager.Resource.LoadAsset<TextAsset>(jsonPath + heroFileName);
        var jsons = JsonHelper.FromJson<HeroData>(json.text);
        foreach (var hero in jsons)
        {
            int grade = hero.heroId / 1000000 % 10;
            Grade heroGrade = (Grade)grade;

            heroDataBase.Add(hero.heroId, hero);
            gradeHeroData[heroGrade].Add(hero.heroId);
        }
    }

    private void MonsterSheetParse()
    {
        var json = GameManager.Resource.LoadAsset<TextAsset>(jsonPath + monsterFileName);
        var jsons = JsonHelper.FromJson<MonsterData>(json.text);
        foreach (var monster in jsons)
        {
            monsterDataBase.Add(monster.monsterId, monster);
        }
    }

    private void WaveSheetParse()
    {
        var json = GameManager.Resource.LoadAsset<TextAsset>(jsonPath + waveFileName);
        var jsons = JsonHelper.FromJson<MonsterWaveData>(json.text);
        foreach (var wave in jsons)
        {
            if (!waveDataBase.ContainsKey(wave.roundNum))
            {
                List<MonsterWaveData> waveDataList = new();
                waveDataBase.Add(wave.roundNum, waveDataList);
            }
            
            waveDataBase[wave.roundNum].Add(wave);
        }
    }
    
    public MonsterData GetMonsterByID(int id)
    {
        var monster = monsterDataBase.TryGetValue(id, out MonsterData data) ? data : null;
        if (monster == null)
        {
            Logger.WarningLog($"[DataManager] none of monster id : {id}");
            return null;
        }

        return monster;
    }

    public HeroData GetHeroByID(int id)
    {
        var hero =  heroDataBase.TryGetValue(id, out HeroData data) ? data : null;
        if (hero == null)
        {
            Logger.WarningLog($"[DataManager] none of hero id : {id}");
            return null;
        }

        return hero;
    }

    public List<HeroData> GetHerosByGrade(Grade grade)
    {
        List<int> heroIds = gradeHeroData.TryGetValue(grade, out List<int> list) ? list : null;
        if (heroIds == null)
        {
            Logger.WarningLog($"[DataManager] none of grade : {grade}");
            return null;
        }

        List<HeroData> heros = new();
        foreach (var id in heroIds)
        {
            var hero = GetHeroByID(id);
            if (hero != null)
            {
                heros.Add(hero);
            }
        }
        
        return heros;
    }

    public List<MonsterWaveData> GetWaveDataByRound(int round)
    {
        List<MonsterWaveData> monsterWaves = waveDataBase.TryGetValue(round, out List<MonsterWaveData> list) ? list : null;
        if (monsterWaves == null || monsterWaves.Count == 0)
        {
            Logger.WarningLog($"[DataManager] none of round : {round}");
            return null;
        }
        
        return monsterWaves;
    }
}
