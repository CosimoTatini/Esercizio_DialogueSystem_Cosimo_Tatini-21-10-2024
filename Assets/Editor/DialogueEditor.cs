using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    private DialogueDataScriptableObject _dialogueData;

    [MenuItem("Window/DialogueEditor")]
    public static void ShowWindow()
    {
        GetWindow<DialogueEditor>("Dialogue Editor");
    }

    private void OnGUI()
    {
       if(_dialogueData==null)
       {
         if (GUILayout.Button("Create New Dialogue"))
         {
          _dialogueData = new DialogueDataScriptableObject();
         }
       }
       else
       {
            GUILayout.Label("Dialogue Editor", EditorStyles.boldLabel);
            
            if(GUILayout.Button("Add Line"))
            {
                EditorLineWindow.OpenWindow(_dialogueData);
            }
            foreach(var line in _dialogueData.lines)
            {
                GUILayout.Button(line.text);
            }
            if (GUILayout.Button("Save Dialogue as ScriptableObject"))
            {
                SaveDialogueData();
            }
       }
    }

    private void SaveDialogueData()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Dialogue", "new Dialogue", "asset", "Please enter a file name to save the dialogue");
        
        if(!string.IsNullOrEmpty(path))
        {
         AssetDatabase.CreateAsset(_dialogueData, path);
         AssetDatabase.SaveAssets();
         AssetDatabase.Refresh();
        }
      
    }
}
