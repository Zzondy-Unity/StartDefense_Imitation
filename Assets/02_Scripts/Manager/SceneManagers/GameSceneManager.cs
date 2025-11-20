using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private Tilemap tilemap;
    
    public TileManager Tile { get; private set; }
    public WaveManager Wave { get; private set; }

    public override SceneName curScene { get;} = SceneName.GameScene;

    private Coroutine gameFlowCoroutine;

    public override void Init()
    {
        base.Init();
        
        if (tilemap == null)
        {
            if (GameObject.FindGameObjectWithTag("Tilemap").TryGetComponent<Tilemap>(out Tilemap tile))
            {
                tilemap = tile;
            }
            else
            {
                Logger.ErrorLog($"Tilemap component not found in scene.");
                return;
            }
        }
        Tile = new TileManager(tilemap);
        Wave = GetComponentInChildren<WaveManager>();
    }

    public override void OnEnter()
    {
        gameFlowCoroutine = StartCoroutine(GameStart());
    }

    public override void OnExit()
    {
        StopCoroutine(gameFlowCoroutine);
        gameFlowCoroutine = null;
    }

    private IEnumerator GameStart()
    {
        // wave 시작 대기
        yield return null;
        
        // wave 시작

        yield return null;
        
        // 몬스터 일정 주기로 스폰

        yield return null;
        
        // 모든 몬스터 잡았을 때, 게임 승패 로직 추가
    }
}
