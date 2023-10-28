using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField] private Toggle _pauseButton;
    [SerializeField] private TimeCounter _timeCounter;
    public override void Initialize()
    {
        _pauseButton.onValueChanged?.AddListener((x) =>
        {
            if (x)
                ViewManager.Show<PauseView>(true, ViewType.ADD);
            else
                ViewManager.Show<GameView>();

        });

    }
    public void SetTime(int time)
    {
        _timeCounter.timeText.text = time.ToString();
    }
}
