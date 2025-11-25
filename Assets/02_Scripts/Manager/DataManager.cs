using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : IManager
{
    private PlayerData playerData;
    
    private readonly Dictionary<int, MonsterData> monsterDataBase = new();          // id에 해당하는 몬스터정보
    private readonly Dictionary<int, HeroData> heroDataBase = new();                // id에 해당하는 영웅정보
    private readonly Dictionary<int, List<MonsterWaveData>> waveDataBase = new();   // round별 웨이브 정보
    private readonly Dictionary<int, List<int>> trescendHeroDic = new();
    private readonly List<int> bountyList = new();

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
    
    private readonly string playerFileName = "playerData.json";
    
    public void Init()
    {
        HeroSheetParse();
        MonsterSheetParse();
        WaveSheetParse();
        
        LoadPlayerData();
    }

    #region <<<<<<<< 시트 파싱 >>>>>>>>
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
            // 초월체 정보 저장
            if (heroGrade == Grade.epic)
            {
                if (!trescendHeroDic.ContainsKey(hero.heroId))
                {
                    List<int> newTrescends = new();
                    trescendHeroDic.Add(hero.heroId, newTrescends);
                }
                
                int[] trecends = hero.transcendList;
                for (int i = 0; i < trecends.Length; i++)
                {
                    trescendHeroDic[hero.heroId].Add(trecends[i]);
                }
            }
        }
    }

    private void MonsterSheetParse()
    {
        var json = GameManager.Resource.LoadAsset<TextAsset>(jsonPath + monsterFileName);
        var jsons = JsonHelper.FromJson<MonsterData>(json.text);
        foreach (var monster in jsons)
        {
            monsterDataBase.Add(monster.monsterId, monster);
            if (monster.monsterId / 1000000 % 10 == 3)
            {
                bountyList.Add(monster.monsterId);
            }
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
    

    #endregion

    #region <<<<<<<< public함수 >>>>>>>>

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

    public int GetHeroByGradeRandom(Grade grade)
    {
        List<int> heroIds = gradeHeroData.TryGetValue(grade, out List<int> list) ? list : null;
        if (heroIds == null)
        {
            return 0;
        }
        
        int id = list[Random.Range(0, list.Count)];
        return id;
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

    public List<int> GetBountyMonster()
    {
        return bountyList;
    }

    #endregion

    #region <<<<<<<< 저장 & 로드 >>>>>>>>

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void SavePlayerData()
    {
        string toJson = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.persistentDataPath + playerFileName);
        File.WriteAllText(path, toJson);
    }

    public void LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath + playerFileName);
        if (!File.Exists(path))
        {
            playerData = new PlayerData();
            return;
        }
        
        string json = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(json);
    }

    #endregion

    
}
