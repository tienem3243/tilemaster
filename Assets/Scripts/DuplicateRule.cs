using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateRule : Gamerule
{
    int amountDupRequire = 3;

    public DuplicateRule(int amountDupRequire)
    {
        this.amountDupRequire = amountDupRequire;
    }

    public DuplicateRule()
    {
    }

    public bool RuleCheck(Tile[] tiles)
    {
        foreach (var item in tiles)
        {
           if( item.tileName == tiles[0].tileName) return false;
        }
        return true;
    }
}
