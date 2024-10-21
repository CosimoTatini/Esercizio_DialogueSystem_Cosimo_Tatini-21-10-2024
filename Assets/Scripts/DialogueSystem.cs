using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public Image portraitImage;
    public TextMeshProUGUI dialogueText;
    public DialogueDataScriptableObject dialogueData;
    private int _currentLineIndex = 0;
    private bool _isDisplayingText = false;
    private Coroutine _typingCoroutine;

    private void Start()
    {
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (_currentLineIndex < dialogueData.lines.Count)
        {
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _typingCoroutine = StartCoroutine(TypeLine(dialogueData.lines[_currentLineIndex].text));
            _currentLineIndex++;
        }

    }
    public void SkipLine()
    {
        if (_isDisplayingText)
        {
            StopCoroutine(_typingCoroutine);

            portraitImage.sprite = dialogueData.lines[_currentLineIndex].portrait;
            dialogueText.text = dialogueData.lines[_currentLineIndex - 1].text;
            _isDisplayingText = false;
        }
        else
        {
            DisplayNextLine();
        }
    }
    public void PreviousLine()
    {
        if (_currentLineIndex > 1)
        {
            _currentLineIndex -= 2;
            DisplayNextLine();

        }
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        _isDisplayingText = true;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        _isDisplayingText = false;

    }
}
