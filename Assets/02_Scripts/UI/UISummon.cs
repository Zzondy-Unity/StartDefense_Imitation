using UnityEngine;

public class UISummon : UIPopUp
{
    [SerializeField] private atom_buy upgrade;
    private TileNode curTile;

    public override void Opened(object[] param)
    {
        base.Opened(param);
        
        if (gameSceneManager != null && param != null && param[0] is TileNode tile)
        {
            curTile = tile;
            upgrade.SetAtom(MoneyType.gold, gameSceneManager.SummonCost, UnitPopupMode.Summon, Summon, () =>
            {
                GameManager.UI.Hide<UISummon>();
            });

            Refresh();
        }
    }

    private void Summon()
    {
        if (gameSceneManager != null)
        {
            gameSceneManager.Stage.SpawnHero(Grade.epic, curTile);
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        
        upgrade.Refresh(gameSceneManager.Stage.canSummon);
    }
}