using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Hero", menuName = "GameData/CreateHeroData")]

public class HeroSO : ScriptableObject
{
	public int heroId;
	public float attackRate;
	public float attack;
	public float attackRange;
	public string resourceKey;
	public int combinationHero;
	public string bulletKey;

}
