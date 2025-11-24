using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIPopUp
{
    [SerializeField] private atom_btn upgradeBtn;
    private TileNode _tile;
    
    public override void Opened(object[] param)
    {
        base.Opened(param);
        if (param[0] is TileNode tile)
        {
            _tile = tile;
            upgradeBtn.Set(UnitPopupMode.Upgrade.ToString(), OnClick, Color.blue, Color.white );
            Refresh();
        }
    }

    private void OnClick()
    {
        if (_tile == null) return;
        gameSceneManager.Stage.UpgradeHero(_tile);
        _tile = null;
        Hide<UIUpgrade>();
    }

    public override void Refresh()
    {
        base.Refresh();

        if (_tile != null)
        {
            upgradeBtn.interactable = gameSceneManager.Stage.CanUpgrade(_tile.curHero.id);
        }
        else
        {
            Hide<UIUpgrade>();
        }
    }
}
