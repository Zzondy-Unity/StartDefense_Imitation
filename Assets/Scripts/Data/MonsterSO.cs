using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Monster", menuName = "GameData/CreateMonsterData")]

public class MonsterSO : ScriptableObject
{
	public int monsterId;
	public string monsterName;
	public int maxHealth;
	public float moveSpeed;
	public string resourceKey;
	public int rewardGold;
	public int rewardMineral;
	public string monsterDesc;
	public string spriteKey;

}
