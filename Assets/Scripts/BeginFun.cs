using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginFun : MonoBehaviour
{

    public void PlayStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void PlayEnd()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
