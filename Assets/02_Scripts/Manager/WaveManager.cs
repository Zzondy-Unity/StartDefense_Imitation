using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private List<Monster> curWave = new List<Monster>();
    
    public void MonsterSpawn(int id)
    {
        var monster = GameManager.Resource.LoadAsset<Monster>("");
        //  monster 데이터 적용 및 오브젝트 풀링 이용
        
        
        
    }
}
