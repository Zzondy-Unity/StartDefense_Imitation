using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "MonsterWave", menuName = "GameData/CreateMonsterWaveData")]

public class MonsterWaveSO : ScriptableObject
{
	public int roundNum;
	public int waveIndex;
	public int totalSpawn;
	public int spawnMonsterId;

}
