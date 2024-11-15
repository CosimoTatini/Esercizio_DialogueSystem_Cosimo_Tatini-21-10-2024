using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        if (_dialogueData == null)
        {
            GUILayout.Label("No dialogue loaded", EditorStyles.boldLabel);

            if (GUILayout.Button("Create New Dialogue"))
            {
                _dialogueData = CreateInstance<DialogueDataScriptableObject>();
            }

            if (GUILayout.Button("Load Existing Dialogue"))
            {
                LoadDialogueData();
            }
        }
        else
        {
            GUILayout.Label("Dialogue Editor", EditorStyles.boldLabel);

            // Aggiungi nuova linea di dialogo
            if (GUILayout.Button("Add Line"))
            {
                _dialogueData.lines.Add(new DialogLine());
            }

            // Ciclo per mostrare e modificare ogni linea
            for (int i = 0; i < _dialogueData.lines.Count; i++)
            {
                GUILayout.BeginHorizontal();

                // Mostra il campo di input per il testo della linea
                _dialogueData.lines[i].text = EditorGUILayout.TextField($"Line {i + 1}", _dialogueData.lines[i].text);

                // Pulsante per rimuovere la linea
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    _dialogueData.lines.RemoveAt(i);
                    i--;
                }

                GUILayout.EndHorizontal();
                continue;
            }

            // Pulsante per salvare il dialogo come ScriptableObject
            if (GUILayout.Button("Save Dialogue as ScriptableObject"))
            {
                SaveDialogueData();
            }

            // Pulsante per svuotare il dialogo
            if (GUILayout.Button("Clear Dialogue"))
            {
                _dialogueData = null;
            }
            if (GUILayout.Button("Delete Dialogue Data"))
            {
                DeleteDialogueData();
            }
        }
    }

    private void SaveDialogueData()
    {
        if (_dialogueData != null)
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Dialogue", "NewDialogue", "asset", "Please enter a file name to save the dialogue");

            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(_dialogueData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }

    private void LoadDialogueData()
    {
        string path = EditorUtility.OpenFilePanel("Load Dialogue", "Assets", "asset");

        if (!string.IsNullOrEmpty(path))
        {
            path = FileUtil.GetProjectRelativePath(path);
            _dialogueData = AssetDatabase.LoadAssetAtPath<DialogueDataScriptableObject>(path);

            if (_dialogueData == null)
            {
                EditorUtility.DisplayDialog("Error", "Selected file is not a valid Dialogue Data asset", "OK");
            }
        }
    }
    private void DeleteDialogueData()
    {
        if (_dialogueData != null)
        {
            string path = AssetDatabase.GetAssetPath(_dialogueData);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                _dialogueData = null;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success", "Dialogue asset deleted", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "No valid asset path found for deletion", "OK");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "No dialogue asset loaded to delete", "OK");
        }
    }
}
