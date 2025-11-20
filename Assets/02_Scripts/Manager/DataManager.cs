using System.Collections.Generic;
using UnityEngine;

public class DataManager : IManager
{
    private Dictionary<int, Monster> monsterData = new Dictionary<int, Monster>();
    
    public void Init()
    {
        
    }

    // Monster대신 MonsterData로써 건내줄 예정
    public Monster GetMonsterByID(int id)
    {
        return monsterData.TryGetValue(id, out Monster monster) ? monster : null;
    }
}
