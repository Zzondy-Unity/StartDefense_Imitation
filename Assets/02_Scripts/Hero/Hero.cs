using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HeroAttackController))]
public class Hero : MonoBehaviour , IPoolable, IAttacker
{
    [SerializeField] private Image canUpImage;
    [SerializeField] private Image gradeRing;
    
    public HeroData data { get; private set; }
    public int id { get; private set; }
    public Grade heroGrade { get; private set; }
    public TileNode tile { get; private set; }
    
    private HeroAttackController attackController;
    private Tween shakeTween;
    
    public virtual void Init(HeroData _data, TileNode _tile)
    {
        data = _data;
        id = _data.heroId;
        heroGrade = (Grade)(id / 1000000 % 10);
        tile = _tile;

        if (TryGetComponent<HeroAttackController>(out var controller))
        {
            bool isBuffed = _tile.tileType == TileType.Buffed;
            attackController = controller;
            attackController.Init(data, this, isBuffed);
            if (isBuffed)
            {
                shakeTween = transform.DOShakeScale(1.5f, 1f).SetLoops(-1);
            }
        }
        
        canUpImage.gameObject.SetActive(false);
        gradeRing.color = Utility.GetColorByGrade(heroGrade);
    }

    public void Enhance()
    {
        attackController.Enhance();
        transform.DOScaleX(5, 1)
            .OnComplete(() => transform.DOScaleX(1, 1));
    }
    
    public Component poolKey { get; set; }
    public virtual void OnSpawnFromPool()
    {
        
    }

    public virtual void OnReturnToPool()
    {
        if (shakeTween != null)
        {
            shakeTween.Kill();
        }
        tile.curHero = null;
        attackController.OnDead();
    }
}