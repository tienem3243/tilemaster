using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LevelSelectButton : MonoBehaviour
{
    public string lvName;
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        text.text = lvName;
        GetComponent<Button>().onClick?.AddListener(()=>TileManager.Instance.LoadLevel(lvName));
    }
}
