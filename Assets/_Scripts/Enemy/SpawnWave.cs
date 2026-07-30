using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnWave
{
    [Header("このWaveが有効な時間")]
    public float StartTime;
    public float EndTime;
    [Header("1秒間に何体スポーンするか")]
    public float SpawnPerSecond = 1;
    [Header("出現する敵")]
    public List<SpawnEnemyData> Enemies = new();
}