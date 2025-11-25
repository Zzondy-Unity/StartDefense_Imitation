using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 관련된 것들을 관리합니다.
/// </summary>
public class WaveManager : MonoBehaviour
{
    private GameSceneManager manager;

    public float curTime { get; private set; }
    public int curWaveNumber { get; private set; }
    public int maxWaveNumber { get; private set; }

    // 몬스터 정보를 담아두고있는 리스트의 카운트로 변경
    public int curMonsterNumber
    {
        get { return monsterNum; }

        private set
        {
            monsterNum = value;
        }
    }

    private int curRound;
    private int monsterNum = 0;

    private Coroutine timeCoroutine;
    private Coroutine waveLoop;

    private readonly float maxWaitForNexWaveTime = 10f;
    private readonly float spawnInterval = 0.5f;

    private Dictionary<int, MonsterWaveData> curWaveDatas = new();

    public void Init(int roundNumber, GameSceneManager gameSceneManager)
    {
        manager = gameSceneManager;

        // 시간 및 웨이브정보 리셋
        ResetTime();
        curWaveNumber = 0;
        maxWaveNumber = 0;

        // 라운드정보 초기화
        curRound = roundNumber;
        
        // 라운드별 웨이브 데이터 초기화
        curWaveDatas.Clear();
        var waveDatas = GameManager.Data.GetWaveDataByRound(roundNumber);
        foreach (var waveData in waveDatas)
        {
            if (curRound == waveData.roundNum)
            {
                curWaveDatas.Add(waveData.waveIndex, waveData);
            }
        }

        // 이벤트 연결
        EventManager.Subscribe(GameEventType.MonsterDeath, OnMonsterDie);
    }

    public void OnGameEnd(bool isWin)
    {
        if (waveLoop != null)
        {
            StopCoroutine(waveLoop);
            waveLoop = null;
        }

        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        EventManager.UnSubscribe(GameEventType.MonsterDeath, OnMonsterDie);
    }

    public void RoundStart()
    {
        ResetTime();
        timeCoroutine = StartCoroutine(StartTime());

        if (waveLoop != null)
        {
            StopCoroutine(waveLoop);
            waveLoop = null;
        }

        curWaveNumber = 1;
        maxWaveNumber = curWaveDatas.Count;

        waveLoop = StartCoroutine(WaveLoopStart());
    }

    private IEnumerator WaveLoopStart()
    {
        while (curWaveNumber <= maxWaveNumber)
        {
            // 몬스터가 다잡히고 일정시간(10초)후에 스폰이 시작되거나
            // 유저가 직접 눌러서 스폰이 시작됨 -- 어떻게하지
            int monsterId = curWaveDatas[curWaveNumber].spawnMonsterId;
            int monsterNum = curWaveDatas[curWaveNumber].totalSpawn;
            for (int i = 0; i < monsterNum; i++)
            {
                SpawnMonster(monsterId);

                // TODO :: WaitForSeconds 미리 캐싱해둘것 (너무 자주 사용)
                yield return new WaitForSeconds(spawnInterval);
            }
            
            curWaveNumber++;
            yield return new WaitForSeconds(maxWaitForNexWaveTime);
        }
        
        waveLoop = null;
    }

    public void SpawnMonster(int id)
    {
        MonsterData monsterData = GameManager.Data.GetMonsterByID(id);
        Monster monsterAsset = GameManager.Resource.LoadAsset<Monster>(monsterData.resourceKey);
        var monster = GameManager.Pool.GetFromPool(monsterAsset);
        monster.Init(monsterData);
        monster.transform.position = manager.Tile.GetSpawnPosition();
        curMonsterNumber++;
    }

    private void OnMonsterDie(object org)
    {
        curMonsterNumber--;
        if (curMonsterNumber <= 0 && curWaveNumber > maxWaveNumber)
        {
            EventManager.Publish(GameEventType.GameEnd, true);
        }
    }

    private void ResetTime()
    {
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
        }

        timeCoroutine = null;
        curTime = 0;
    }

    private IEnumerator StartTime()
    {
        curTime = Time.deltaTime;
        yield return null;
    }
}