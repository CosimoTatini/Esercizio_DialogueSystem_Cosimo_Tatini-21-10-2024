using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue" , menuName = "DialogueSystem/Dialogue")]
public class DialogueDataScriptableObject:ScriptableObject
{
    public List<DialogLine> lines = new List<DialogLine>();
}

[System.Serializable]
public class DialogLine
{
    public string text;
    public Sprite portrait;
    public int characterID;
}