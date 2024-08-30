using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/GameData")]

public class LevelDataSO : ScriptableObject
{
    public int level;
    public int colorAmount;
    public int bottleAmount;
}
