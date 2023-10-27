using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LvConfig", menuName = "ScriptableObjects/LvConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public int maxAmount = 33;
    public List<TileConfig> tileConfigs;

    public double CalculateNormalize(TileConfig tile)
    {
        double rate = Convert.ToDouble(tile.chance) / Convert.ToDouble(GetTotalRate());
        return rate;
    }  
    public int GetTotalRate()
    {
        int total = 0;
        tileConfigs.ForEach(config => total += config.chance);
        return total;
    }
}
[System.Serializable]
public struct TileConfig
{
    public string tileName;
    public Sprite sprite;
    public int chance;
}
