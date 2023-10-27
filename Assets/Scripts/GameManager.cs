using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] int totalTileCount;
    [SerializeField]int playTime;
    [SerializeField] int maxPlayTime=12;

    private bool isPause;
    

    private void Start()
    {
        isPause = true;
        StartCoroutine(TimeCounter());
    }

  
    private IEnumerator TimeCounter()
    {
        while (playTime < maxPlayTime)
        {
            
            playTime+=isPause?0:1;
            yield return new WaitForSeconds(1);
        }
        Lose();
    }

    public void Lose()
    {
        isPause = true;
        Debug.Log("Lose");
    }
    public void Win()
    {
        isPause = true;
        Debug.Log("Win");
    }
    public void Reset()
    {
        isPause = true;
        playTime = 0;
    }
}
