using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HeroAttackController))]
public class Hero : MonoBehaviour , IPoolable, IAttacker
{
    public HeroData data { get; private set; }
    public int id { get; private set; }
    public Grade heroGrade { get; private set; }
    public TileNode tile { get; private set; }

    private HeroAttackController attackController;
    
    [SerializeField] private Image canUpImage;
    [SerializeField] private Image gradeRing;

    public bool CanUpgrade
    {
        get
        {
            var manager = GameManager.Scene.curSceneManager as GameSceneManager;
            return manager.Stage.CanUpgrade(data.heroId);
        }
        set
        {
            ShowUpgrade(value);
        }
    }

    public virtual void Init(HeroData _data, TileNode _tile)
    {
        data = _data;
        id = _data.heroId;
        heroGrade = (Grade)(id / 1000000 % 10);
        tile = _tile;

        if (TryGetComponent<HeroAttackController>(out var controller))
        {
            attackController = controller;
            attackController.Init(data, this);
        }
        
        canUpImage.gameObject.SetActive(false);
        gradeRing.color = Utility.GetColorByGrade(heroGrade);
    }

    public virtual void ShowUpgrade(bool canUpgrade)
    {
        canUpImage.gameObject.SetActive(canUpgrade);
    }

    public Component poolKey { get; set; }
    public virtual void OnSpawnFromPool()
    {
        
    }

    public virtual void OnReturnToPool()
    {
        tile.curHero = null;
        attackController.OnDead();
    }
}