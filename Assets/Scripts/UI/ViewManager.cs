using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviourSingleton<ViewManager>  
{
    
    [SerializeField]private View _startingView;
    [SerializeField]private View[] _views;
    [SerializeField] private View _currentView;
    private void Start()
    {
        foreach (var item in _views)
        {
            item.Initialize();
            item.Hide();    
        }
        if (_startingView != null) Show(_startingView, true);
    }

    private readonly Stack<View> _history= new Stack<View> ();

    public static T GetView<T>() where T : View
    {
        for (int i = 0; i < Instance._views.Length; i++)
        {
            if (Instance._views[i] is T tview)
            {
                return tview;
            }
        }
        return null;
    }
    public static void Show<T>(bool remember=true, ViewType type=ViewType.REPLACE) where T : View
    {
        for (int i = 0; i < Instance._views.Length; i++)
        {
            if (Instance._views[i] is T)
            {
                if (Instance._currentView != null)
                {
                    if (remember)
                    {
                        Instance._history.Push(Instance._currentView);  
                    }
                    if(type==ViewType.REPLACE)
                    Instance._currentView.Hide();
                }
                Instance._views[i].Show();
                Instance._currentView = Instance._views[i];
            }
        }
    }
    public static void Show(View view,bool remember=true)
    {
        if(Instance._currentView != null)
        {
            if (remember)
            {
                Instance._history.Push(Instance._currentView);
            }
            Instance._currentView.Hide();
            Instance._currentView = view;
        }
        Instance._currentView = view;
        view.Show();
    }
    public static void ShowLast()
    {
        if( Instance._history.Count !=0)
        {
            Show(Instance._history.Pop(), false);
        }
    }
  


   
}
