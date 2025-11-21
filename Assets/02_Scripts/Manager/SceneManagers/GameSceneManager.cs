using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform[] waypointTransforms;

    public int RoundNumber { get; } = 1;
    
    public TileManager Tile { get; private set; }
    public WaveManager Wave { get; private set; }

    public override SceneName curScene { get;} = SceneName.GameScene;

    public override void Init()
    {
        base.Init();
        
        Logger.Log($"[GameSceneManager] GameScene Initialized.");
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

        Vector3[] waypoints = new Vector3[waypointTransforms.Length];
        for (int i = 0; i < waypointTransforms.Length; i++)
        {
            waypoints[i] = waypointTransforms[i].position;
        }
        Tile = new TileManager(tilemap, waypoints, this);
        Wave = GetComponentInChildren<WaveManager>();
        Wave.Init(RoundNumber, this);
    }

    public override void OnEnter()
    {
        Logger.Log($"[GameSceneManager] GameScene Loaded and OnEntered. Round start!");
        Wave.RoundStart();
    }

    public override void OnExit()
    {
        
    }
}
