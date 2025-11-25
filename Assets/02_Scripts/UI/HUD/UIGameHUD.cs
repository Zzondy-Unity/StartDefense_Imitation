using UnityEngine;

public class UIGameHUD : UIBase
{
    [SerializeField] private HUD_TOP top;
    [SerializeField] private HUD_MIDDLE middle;
    [SerializeField] private HUD_BOTTOM bottom;

    public void Init()
    {
        if(top == null) top = GetComponentInChildren<HUD_TOP>();
        if (middle == null) middle = GetComponentInChildren<HUD_MIDDLE>();
        if (bottom == null) bottom = GetComponentInChildren<HUD_BOTTOM>();
        
        top.Init(this);
        middle.Init(this);
        bottom.Init(this);
    }

    public void ShowBounty()
    {
        middle.ShowBountyUI();
    }

    public void HideBounty()
    {   
        middle.HideBountyUI();
    }

    public override void Opened(object[] param)
    {
        base.Opened(param);

        Init();
    }

    public override void Refresh()
    {
        base.Refresh();
        
        top.Refresh();
        middle.Refresh();
        bottom.Refresh();
    }

    public void ShowEnhance()
    {
        middle.ShowEnhanceUI();
    }

    public void HideEnhance()
    {
        
    }
}
