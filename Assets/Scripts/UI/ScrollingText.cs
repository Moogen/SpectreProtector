using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Takes the text in a TextMeshProUGUI component and causes it to scroll across the screen. 
 * Multiple blocks of text can be delimited using ; (or whatever the user-assigned delimiter is).
 * In order to make this look good, the TextMeshProUGUI component must be vertically aligned to the top. 
 * Additionally, all line breaks must be manually put in. This prevents the text from typing out partially in one line 
 * and then wrapping when it gets too long for the text box. 
 * 
 * By default, text block transitions will be triggered when the user presses the Space bar or clicks the left mouse button. 
 * This can be changed in the waitForNext() function.
 * 
 * If the scene is part of a set of scene transitions, the nextScene field can be filled out and the next scene will load 
 * after the final code block when the user triggers next. Otherwise, just leave the field blank. 
 */
public class ScrollingText : MonoBehaviour
{
    [SerializeField] private char delimiter = ';';
    [SerializeField] private string nextScene;
    private TextMeshProUGUI tmp; 

    private void Start()
    {
        if (tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }
        string desiredText = tmp.text;
        string[] fullText = desiredText.Split(delimiter);
        tmp.text = "";
        StartCoroutine(fullScrollText(fullText));
    }

    private IEnumerator fullScrollText(string[] fullText)
    {
        foreach (string s in fullText)
        {
            foreach (char c in s)
            {
                tmp.text += c;
                yield return new WaitForSeconds(0.05f);
            }
            yield return StartCoroutine(waitForNext());
            tmp.text = "";
        }
        if (nextScene != "")
        {
            LaunchScenes.Instance.LaunchScene(nextScene);
        }
    }

    private IEnumerator waitForNext()
    {
        bool wait = true;
        while (wait)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                wait = false;
            }
            yield return null;
        }
    }
}