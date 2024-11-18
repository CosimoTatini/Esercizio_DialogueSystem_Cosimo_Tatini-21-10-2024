using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public Image portraitImage1;
    public Image portraitImage2;
    public TextMeshProUGUI dialogueText;
    public DialogueDataScriptableObject dialogueData;
    [SerializeField] private int _currentLineIndex = 0;
    [SerializeField] private bool _isDisplayingText = false;
    public bool IsDisplayingText {
        get { return _isDisplayingText; }
        set
        {
            Debug.Log("Is Displaying Text: " + value);
            _isDisplayingText = value;
        } }
        

    private Coroutine _typingCoroutine;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private void Start()
    {
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (_currentLineIndex < dialogueData.lines.Count)
        {
            if (IsDisplayingText) 
                CancelTypingTask();
            
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

            // _typingCoroutine = StartCoroutine(TypeLine(dialogueData.lines[_currentLineIndex].text));
            // _cancellationTokenSource = new CancellationTokenSource();
            TypeLineTask(dialogueData.lines[_currentLineIndex].text, _cancellationTokenSource.Token).Forget();

            _currentLineIndex++;
        }

    }
    public void SkipLine()
    {
        if (_isDisplayingText)
        {
            CancelTypingTask();
            dialogueText.text = dialogueData.lines[_currentLineIndex - 1].text;
            IsDisplayingText = false;
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

    private async UniTask TypeLineTask(string line, CancellationToken cancellationToken)
    {

        try
        {
            dialogueText.text = "";
            IsDisplayingText = true;

            foreach (char letter in line.ToCharArray())
            {
                cancellationToken.ThrowIfCancellationRequested();
                dialogueText.text += letter;
                await UniTask.WaitForSeconds(0.05f, cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Ex: " + ex);
            // IsDisplayingText = false;
        }

        // IsDisplayingText = false;
    }

    private void CancelTypingTask()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();

        IsDisplayingText = false;
    }
}