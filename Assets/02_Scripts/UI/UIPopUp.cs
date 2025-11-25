using UnityEngine;

public class UIPopUp : UIBase
{
    protected GameSceneManager gameSceneManager => GameManager.Scene.curSceneManager as GameSceneManager;
    
    public override void Opened(object[] param)
    {
        base.Opened(param);
        
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

        if (GameManager.UI.IsOpened<UIFixTile>() != null)
        {
            GameManager.UI.Hide<UIFixTile>();
        }
    }

    protected void Hide<T>() where T : UIPopUp
    {
        GameManager.UI.Hide<T>();
    }
}
