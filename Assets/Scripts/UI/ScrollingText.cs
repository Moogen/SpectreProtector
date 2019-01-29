using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public string nextScene;
    [SerializeField] GameObject skipButton;
    TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        string desiredText = tmp.text;

        string[] fullText = desiredText.Split('\n');

        /*
        tmp.outlineColor = Color.black;
        tmp.outlineWidth = 0.5f;
        */

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
            yield return new WaitForSeconds(0.05f);
            tmp.text += "\n";
        }
        /* Not convinced we actually want this functionality
        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }
        */
        yield return StartCoroutine(waitForSpace(true));
        if (!(SceneManager.GetActiveScene().name == "TryAgain"))
        {
            LaunchScenes.Instance.LaunchScene(nextScene);
        }
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