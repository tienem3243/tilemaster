using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectView : View
{
    public LevelSelectButton prefabBtn;
    public Transform root;

    public override void Initialize()
    {
        for (int i = 0; i < 3; i++)
        {
            var obj = Instantiate(prefabBtn, root) as LevelSelectButton;
            obj.lvName = i.ToString();
            obj.gameObject.GetComponent<Button>().onClick?.AddListener(() => ViewManager.Show<GameView>());

        }
    }

}
