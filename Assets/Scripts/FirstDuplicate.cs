using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// check the first collection of continious duplicate tiles
/// </summary>
public class FirstDuplicate : Gamerule
{
    int amountDupRequire = 3;

    public FirstDuplicate(int amountDupRequire)
    {
        this.amountDupRequire = amountDupRequire;
    }

    public int[] GetParameter()
    {
        return new int[] {amountDupRequire};
    }

    //return the valid array of tile
    public bool RuleCheck(Tile[] tiles,out int[] value)
    {
        string tileName = tiles[0].tileName;
        int counter = 0;
     
        int[] tmp = new int[amountDupRequire];
        for (int i =0; i < tiles.Length; i++)
        {
            Debug.Log(tileName);
            if (tiles[i].tileName == tileName)
            { 
                tmp[counter] = i;
                counter++;
            }
            else
            {
                counter = 1;        
                Array.Clear(tmp, 0, amountDupRequire);
                tileName = tiles[i].tileName;
                tmp[0] = i;
            }
            if (counter == amountDupRequire)
            {
                value = tmp;
                return true;
            }
        }
        value = new int[amountDupRequire];
        return false;
    }

 
}
