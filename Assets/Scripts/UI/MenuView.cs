using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : View
{
    [SerializeField] private Button startBtn;

    public override void Initialize()
    {
        startBtn.onClick?.AddListener(() => ViewManager.Show<LevelSelectView>());
    }
}
