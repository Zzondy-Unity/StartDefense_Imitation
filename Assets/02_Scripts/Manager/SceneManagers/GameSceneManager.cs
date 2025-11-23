using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform[] waypointTransforms;
    [SerializeField] private int Round = 1;
    [SerializeField] private Camera mainCamera;

    public int RoundNumber {get {return Round;}}
    public int SummonCost { get; } = 20;

    public TileManager Tile { get; private set; }
    public WaveManager Wave { get; private set; }
    public StageManager Stage { get; private set; }

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
        
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        
        TileNode tileNode = Tile.GetTileNode(worldPos);
        if (tileNode != null && tileNode.tileType == TileType.Normal)
        {
            if (GameManager.UI.IsOpened<UISummon>())
            {
                GameManager.UI.Hide<UISummon>();
            }
            tileNode.Interact(screenPos);
        }
    }
    
    public override void OnEnter()
    {
        GameManager.Input.IsMouseLocked = false;

        GameManager.Input.onClick -= OnMouseClick;
        GameManager.Input.onClick += OnMouseClick;
        
        Wave.RoundStart();
    }

    public override void OnExit()
    {
        
    }
}
