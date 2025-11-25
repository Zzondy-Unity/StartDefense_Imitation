using UnityEngine;


public class HUD_MIDDLE : MonoBehaviour
{
    [SerializeField] private UIBounty uiBounty;
    private UIGameHUD _hud;

    public void Init(UIGameHUD hud)
    {
        _hud = hud;
        uiBounty.Init();
    }

    public void Refresh()
    {
        uiBounty.Refresh();
    }

    public void ShowBountyUI()
    {
        uiBounty.Show();
    }
    
    public void HideBountyUI()
    {
        uiBounty.Hide();
    }
}
