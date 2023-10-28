using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : View
{
    [SerializeField] Button next, reload, menu, back;
    private const string LOSE = "You Lose";
    private const string WIN = "You Win";
    private const string PAUSE = "Pause";
    

    [SerializeField] TextMeshProUGUI stateGameText;
   
    

    private void SetAllEnable(bool state)
    {
        next.gameObject.SetActive(GameManager.Instance.HasNext());
        reload.gameObject.SetActive(state);
        menu.gameObject.SetActive(state);
    }

    public void ShowLosePanel()
    {
        reload.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
        stateGameText.gameObject.SetActive(true);
        StartCoroutine(TypeText(stateGameText, LOSE));

    }
    public void ShowPausePanel(bool toggle)
    {
        SetAllEnable(toggle);
        back.gameObject.SetActive(true);
        next.gameObject.SetActive(false);
        stateGameText.gameObject.SetActive(toggle);
        stateGameText.text = PAUSE;
    }
    public void ShowWinPanel()
    {
        SetAllEnable(true);
        back.gameObject.SetActive(false);
        StartCoroutine(TypeText(stateGameText, WIN));

    }
    //type text/second
    private IEnumerator TypeText(TextMeshProUGUI tmpText, string displayText, float typingSpeed = 0.5f)
    {

        tmpText.text = "";

        for (int i = 0; i < displayText.Length; i++)
        {


            tmpText.text += displayText[i];


            yield return new WaitForSeconds(typingSpeed / displayText.Length);
        }
    }

    public override void Initialize()
    {
        stateGameText.text= PAUSE;
        reload.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);

        reload.onClick?.AddListener(() =>
        {
            GameManager.Instance.Reload();
            ViewManager.Show<GameView>();
           
        });
        menu.onClick?.AddListener(() =>
        {
            GameManager.Instance.Clear();
            ViewManager.Show<MenuView>();
        });
        next.onClick?.AddListener(() =>
        {
            GameManager.Instance.NextLv();
            ViewManager.Show<GameView>();
        });
        back.onClick.AddListener(() =>
        {
            ViewManager.ShowLast();
        });
    }
}
