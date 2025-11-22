using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HeroAttackController))]
public class Hero : MonoBehaviour , IPoolable
{
    public HeroData data { get; private set; }
    public int id { get; private set; }
    public Grade heroGrade { get; private set; }

    private HeroAttackController attackController;
    
    [SerializeField] private Image canUpImage;
    [SerializeField] private Image gradeRing;

    public void Init(HeroData _data)
    {
        data = _data;
        id = _data.heroId;
        heroGrade = (Grade)(id / 1000000 % 10);

        if (TryGetComponent<HeroAttackController>(out var controller))
        {
            attackController = controller;
            Init(_data);
        }
        
        canUpImage.gameObject.SetActive(false);
        gradeRing.color = Utility.GetColorByGrade(heroGrade);
    }

    public Component poolKey { get; set; }
    public void OnSpawnFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        attackController.OnDead();
    }
}