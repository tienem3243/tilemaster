using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class TabWindow : EditorWindow
{
    private int numberOfTabs = 1;

    [MenuItem("Window/UI Toolkit/My GUI Window")]
    public static void ShowWindow()
    {
        GetWindow<MyEditorWindow>("My GUI Window");
    }



    private void OnGUI()
    {
        int tab = 0;
        tab = GUILayout.Toolbar(tab, new string[] { "Object", "Bake", "Layers" });
        switch (tab)
        {
            case 0:
            ShowWindow();
                break;
        }
    }
}


