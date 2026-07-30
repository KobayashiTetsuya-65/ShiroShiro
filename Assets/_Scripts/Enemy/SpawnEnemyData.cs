using System;
using UnityEngine;

[Serializable]
public class SpawnEnemyData
{
    public int EnemyID;

    [Min(1)]
    public int Weight = 1;
}