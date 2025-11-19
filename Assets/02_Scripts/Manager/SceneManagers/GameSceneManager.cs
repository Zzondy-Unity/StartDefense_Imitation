using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : BaseSceneManager
{
    [SerializeField] private Tilemap tilemap;
    
    public TileManager Tile { get; private set; }

    public override SceneName curScene { get;} = SceneName.GameScene;

    public override void Init()
    {
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
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
