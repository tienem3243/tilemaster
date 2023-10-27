using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectSpawn : MonoBehaviour
{
    public LevelSelectButton prefabBtn;
    public Transform root;
    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var obj = Instantiate(prefabBtn, root) as LevelSelectButton;
            obj.lvName = i.ToString();
            obj.gameObject.GetComponent<Button>().onClick?.AddListener(() => gameObject.SetActive(false));
        }
      
    }
}
