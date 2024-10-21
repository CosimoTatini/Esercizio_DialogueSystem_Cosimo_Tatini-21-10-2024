using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorLineWindow : EditorWindow 
{
    private string _linetext = "";
    private Sprite _portrait;
    private DialogueDataScriptableObject _dialogueData;

    public static void OpenWindow(DialogueDataScriptableObject data)
    {
        EditorLineWindow window = GetWindow<EditorLineWindow>("Add Dialogue Line");
        window._dialogueData = data;
    }
    private void OnGUI()
    {
        GUILayout.Label("Add New Line", EditorStyles.boldLabel);
        _linetext = EditorGUILayout.TextField("Line Text", _linetext);

        _portrait = (Sprite)EditorGUILayout.ObjectField("Portrait",_portrait,typeof(Sprite),false);

        if(GUILayout.Button("Add Line"))
        {
            _dialogueData.lines.Add(new DialogLine() { text = _linetext });
            EditorUtility.SetDirty(_dialogueData);
            Close();
        }
    }
}
