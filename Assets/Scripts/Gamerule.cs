using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// GetParameter[0] alwayse is reqirement tile amount
/// </summary>
public interface Gamerule  
{
    public bool RuleCheck(Tile[] tiles, out int[] value);
    public int[] GetParameter();
}
