using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Probe : MonoBehaviour
{
    [SerializeField] private SpriteRenderer probeRenderer;
    
    private readonly string probeSpritePath = "Images/Probe";
    private readonly string probeWithMineralSpritePath = "Images/Probe_mineral";
    
    private Vector3 mineralTransform;
    private Vector3 nexusTransform;

    private int minerals = 30;
    private Sequence workSequence;

    public void Init(Vector3 mineral, Vector3 nexus)
    {
        if (probeRenderer == null)
        {
            probeRenderer = GetComponent<SpriteRenderer>();
        }
        mineralTransform = mineral;
        nexusTransform = nexus;
        
        probeRenderer.sprite = GameManager.Resource.LoadAsset<Sprite>(probeSpritePath);
        DoWork();
    }

    /// <summary>
    /// 프로브가 미네랄을 캤으면 true를
    /// 프로브가 넥서스에 도착했으면 false를
    /// </summary>
    /// <param name="getMineral"></param>
    private void ChangeSprite(bool getMineral)
    {
        probeRenderer.sprite = GameManager.Resource.LoadAsset<Sprite>(getMineral?  probeWithMineralSpritePath : probeSpritePath);
    }

    private void DoWork()
    {
        workSequence =  DOTween.Sequence();
        
        ChangeSprite(false);
        
        // 미네랄쪽으로 이동합니다.
        workSequence.Append(transform.DOMove(mineralTransform, 5f)
            .SetEase(Ease.Linear)
            .OnComplete(() => ChangeSprite(true)));
        
        // 넥서스로 이동하고 도착하면 미네랄을 지급합니다.
        workSequence.Append(transform.DOMove(nexusTransform, 5f))
            .SetEase(Ease.Linear);
        workSequence.AppendCallback(() =>
        {
            ChangeSprite(false);
            EventManager.Publish(GameEventType.GetMineral, minerals);
        });
            
        workSequence.SetLoops(-1);   //무한반복
        workSequence.Play();
    }

    public void OnEnd()
    {
        workSequence.Kill();
    }
}
