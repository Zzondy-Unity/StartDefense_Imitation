using UnityEngine;
using UnityEngine.Serialization;


public class HUD_MIDDLE : MonoBehaviour
{
    [FormerlySerializedAs("uiBounty")] [SerializeField] private Mole_Bounty moleBounty;
    [SerializeField] private Mole_Enhance moleEnhance;
    private UIGameHUD _hud;

    public void Init(UIGameHUD hud)
    {
        _hud = hud;
        moleBounty.Init();
        moleEnhance.Init();
        
        HideBountyUI();
        HideEnhanceUI();
    }

    public void Refresh()
    {
        moleBounty.Refresh();
        moleEnhance.Refresh();
    }

    public void ShowBountyUI()
    {
        moleBounty.Show();
    }
    
    public void HideBountyUI()
    {
        moleBounty.Hide();
    }

    public void ShowEnhanceUI()
    {
        moleEnhance.Show();
    }

    public void HideEnhanceUI()
    {
        moleEnhance.Hide();
    }
}
