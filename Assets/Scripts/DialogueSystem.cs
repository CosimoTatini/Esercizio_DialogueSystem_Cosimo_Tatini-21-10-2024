using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public Image portraitImage1;
    public Image portraitImage2;
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
            DialogLine currentLine = dialogueData.lines[_currentLineIndex];

            if (currentLine.characterID == 1)
            {
                SetPortraitVisibility(portraitImage1, true);
                SetPortraitVisibility(portraitImage2, false);
            }
            else if (currentLine.characterID == 2)
            {
                SetPortraitVisibility(portraitImage1, false);
                SetPortraitVisibility(portraitImage2, true);
            }
            _typingCoroutine = StartCoroutine(TypeLine(dialogueData.lines[_currentLineIndex].text));
            _currentLineIndex++;
        }

    }
    public void SkipLine()
    {
        if (_isDisplayingText)
        {
            StopCoroutine(_typingCoroutine);
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
    private void SetPortraitVisibility(Image portrait, bool isActive)
    {
        UnityEngine.Color portraitColor = portrait.color;
        portraitColor.a = isActive ? 1f : 0.5f;
        portrait.color = portraitColor;
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