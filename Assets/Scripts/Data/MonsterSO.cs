using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Monster", menuName = "GameData/CreateMonsterData")]

public class MonsterSO : ScriptableObject
{
	public int monsterId;
	public int maxHealth;
	public float moveSpeed;
	public string resourceKey;

}
