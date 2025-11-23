using UnityEngine;

public class UISummon : UIPopUp
{
    [SerializeField] private atom_buy upgrade;
    private TileNode curTile;

    GameSceneManager manager
    {
        get { return GameManager.Scene.curSceneManager as GameSceneManager; }
    }

    public override void Opened(object[] param)
    {
        base.Opened(param);
        
        if (manager != null && param != null && param[0] is TileNode tile)
        {
            curTile = tile;
            upgrade.SetAtom(MoneyType.gold, manager.SummonCost, UnitPopupMode.Summon, Summon, () =>
            {
                GameManager.UI.Hide<UISummon>();
            });
        }
    }

    private void Summon()
    {
        if (manager != null)
        {
            var hero = manager.Stage.SpawnHero(Grade.common, curTile);
            hero.transform.position = curTile.worldPos;
        }
    }
}