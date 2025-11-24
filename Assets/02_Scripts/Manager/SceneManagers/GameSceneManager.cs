using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform[] waypointTransforms;
    [SerializeField] private int Round = 1;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Commander commander;

    public int RoundNumber {get {return Round;}}
    public int SummonCost { get; } = 20;

    public TileManager Tile { get; private set; }
    public WaveManager Wave { get; private set; }
    public StageManager Stage { get; private set; }
    public Commander Commander => commander;

    public override SceneName curScene { get;} = SceneName.GameScene;

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

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        Vector3[] waypoints = new Vector3[waypointTransforms.Length];
        for (int i = 0; i < waypointTransforms.Length; i++)
        {
            waypoints[i] = waypointTransforms[i].position;
        }
        Tile = new TileManager(tilemap, waypoints, this);
        Wave = GetComponentInChildren<WaveManager>();
        Stage = GetComponentInChildren<StageManager>();
        
        // 초기화 순서
        commander.Init();
        Wave.Init(RoundNumber, this);
        Stage.Init();
    }
    
    private void OnMouseClick(Vector2 screenPos)
    {
        // world position으로 변경
        if (Tile == null) return;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // ui가 띄어져있으면 ui우선
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        
        TileNode tileNode = Tile.GetTileNode(worldPos);
        Vector2 screen = mainCamera.WorldToScreenPoint(tileNode.worldPos);
        if (tileNode != null)
        {
            if (tileNode.tileType == TileType.Normal)
            {
                // if (GameManager.UI.IsOpened<UISummon>())
                // {
                //     GameManager.UI.Hide<UISummon>();
                // }
                tileNode.Interact(screen);
            }
            else
            {
                if (GameManager.UI.IsOpened<UISummon>() != null)
                {
                    GameManager.UI.Hide<UISummon>();
                }

                if (GameManager.UI.IsOpened<UIUpgrade>() != null)
                {
                    GameManager.UI.Hide<UIUpgrade>();
                }

                if (GameManager.UI.IsOpened<UITrescend>() != null)
                {
                    GameManager.UI.Hide<UITrescend>();
                }
            }
        }
    }

    private void OnGameEnd(object org)
    {
        if (org is bool isWin)
        {
            Wave.OnGameEnd(isWin);
            Stage.OnGameEnd(isWin);
            Tile.OnGameEnd(isWin);

            GameManager.UI.Show<UIResult>(isWin);
        }
    }
    
    public override void OnEnter()
    {
        GameManager.Input.IsMouseLocked = false;
        GameManager.Data.LoadPlayerData();

        GameManager.Input.onClick -= OnMouseClick;
        GameManager.Input.onClick += OnMouseClick;

        EventManager.Subscribe(GameEventType.GameEnd, OnGameEnd);

        GameManager.UI.Show<UIGameHUD>();
        Wave.RoundStart();
    }

    public override void OnExit()
    {
        GameManager.Data.SavePlayerData();
        GameManager.Input.IsMouseLocked = true;
        
        EventManager.UnSubscribe(GameEventType.GameEnd, OnGameEnd);
        GameManager.UI.Hide<UIGameHUD>();
    }
}
