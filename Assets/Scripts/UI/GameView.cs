using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField]private Toggle _pauseButton;
    public override void Initialize()
    {
        _pauseButton.onValueChanged?.AddListener((x) =>
        {
            if (x)             
                ViewManager.Show<PauseView>(true,ViewType.ADD);    
            else              
                ViewManager.Show<GameView>();
            
        });

    }
}
