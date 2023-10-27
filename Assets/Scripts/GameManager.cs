using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
  
    [SerializeField]int playTime;
    [SerializeField] int maxPlayTime=12;
    public string currentLv;
    private bool isPause=true;
    Coroutine timeCounter;

    public void Init()
    {
        isPause = false;
        if (timeCounter != null) StopCoroutine(timeCounter);
        timeCounter= StartCoroutine(TimeCounter());
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
        ViewManager.Show<PauseView>(false, ViewType.ADD);
        ViewManager.GetView<PauseView>().ShowLosePanel();
    }
    public void Win()
    {
        isPause = true;
        ViewManager.Show<PauseView>(false, ViewType.ADD);
        ViewManager.GetView<PauseView>().ShowWinPanel();
    }
  
    public void Reload()
    {
        Clear();
        Init();
        TileManager.Instance.LoadLevel(currentLv);
    }
    public void Clear()
    {
        isPause = false;
        playTime = 0;
        FindObjectOfType<TilePicker>()?.Reset();
        TileManager.Instance.Reset();
        

    }
}
