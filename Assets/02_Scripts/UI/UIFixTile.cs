using UnityEngine;

public class UIFixTile : UIPopUp
{
    [SerializeField] private atom_buy fix;
    private TileNode curTile;

    public override void Opened(object[] param)
    {
        base.Opened(param);
        
        if (gameSceneManager != null && param != null && param[0] is TileNode tile)
        {
            curTile = tile;
            fix.SetAtom(MoneyType.mineral, gameSceneManager.TileFixCost, UnitPopupMode.FixTile, Fix, () =>
            {
                GameManager.UI.Hide<UIFixTile>();
            });

            Refresh();
        }
    }

    private void Fix()
    {
        if (gameSceneManager != null)
        {
            EventManager.Publish(GameEventType.GetMineral, -gameSceneManager.TileFixCost);
            gameSceneManager.Tile.ChangeTile(curTile, TileType.Normal);
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        
        fix.Refresh(gameSceneManager.canFixTile);
    }
}