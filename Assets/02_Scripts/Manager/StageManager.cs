using UnityEngine;

public class StageManager : MonoBehaviour, IManager
{
    // 골드, 미네랄, 프로브의 정보를 가지고
    public int curGold { get; private set; }
    public int curMineral { get; private set; }
    public int curProbes { get; private set; }
    public int maxProbes { get; private set; } = 20;
    
    public void Init()
    {
        curGold = 0;
        curMineral = 0;
        curProbes = 0;
        
    }
    
    //
}
