using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform[] waypointTransforms;
    [SerializeField] private int Round;

    public int RoundNumber {get {return Round;}}
    
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

    public override void OnEnter()
    {
        Wave.RoundStart();
    }

    public override void OnExit()
    {
        
    }
}
