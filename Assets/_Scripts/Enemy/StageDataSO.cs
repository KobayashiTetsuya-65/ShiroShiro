using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/StageData")]
public class StageDataSO : ScriptableObject
{
    public List<SpawnWave> Waves = new();
}