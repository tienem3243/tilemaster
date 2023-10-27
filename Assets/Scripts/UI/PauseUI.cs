using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] Button next, reload, menu;
    private const string LOSE = "You Lose";
    private const string WIN = "You Win";
    private const string PAUSE = "Pause";
    private const string RELOAD = "Reload?";
  

    [SerializeField] TextMeshProUGUI stateGameText;
    [SerializeField] TextMeshProUGUI reloadText;


    private void Start()
    {
        SetAllEnable(false);
    }

    private void SetAllEnable(bool state)
    {
        next.gameObject.SetActive(state);
        reload.gameObject.SetActive(state);
        menu.gameObject.SetActive(state);
    }

    public void ShowLosePanel()
    {
        SetAllEnable(false);
        stateGameText.gameObject.SetActive(true);
        reloadText.gameObject.SetActive(true);  
        StartCoroutine(TypeText(stateGameText, LOSE));
        StartCoroutine(TypeText(reloadText, RELOAD));
    }
    public void ShowPausePanel()
    {
        SetAllEnable(false);
        stateGameText.gameObject.SetActive(true);
        StartCoroutine(TypeText(stateGameText, PAUSE));
    }
    public void ShowWinPanel()
    {
        SetAllEnable(true);
        StartCoroutine(TypeText(stateGameText, WIN));

        StartCoroutine(TypeText(reloadText, RELOAD));
    }
    //type text/second
    private IEnumerator TypeText(TextMeshProUGUI tmpText,string displayText,float typingSpeed=0.5f)
    {
       
        tmpText.text = "";

        for (int i = 0; i < displayText.Length; i++)
        {

            
            tmpText.text += displayText[i];

            
            yield return new WaitForSeconds(typingSpeed/displayText.Length);
        }
    }
}
