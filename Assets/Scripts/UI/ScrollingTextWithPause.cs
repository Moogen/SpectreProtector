using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollingTextWithPause : MonoBehaviour
{
    public char delimiter = ';';
    public string nextScene;
    TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
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
            yield return StartCoroutine(waitForSpace(true));
            tmp.text = "";
        }
        LaunchScenes.Instance.LaunchScene(nextScene);
    }

    private IEnumerator waitForSpace(bool wait)
    {
        while(wait)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                wait = false;
            }
            yield return null;
        }
    }
}