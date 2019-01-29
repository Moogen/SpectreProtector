using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReceiveWin : MonoBehaviour
{
    private int arraysGot;
    private static ReceiveWin _instance = null;
    private Animator anmr;
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        arraysGot = 0;
        anmr = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }
    public static ReceiveWin Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this; // Establish this object as the Singleton

        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate
        }
    }

    public void getAWin()
    {
        arraysGot++;
        if (arraysGot == 4)
        {
            src.Play();
            anmr.SetTrigger("End");
            StartCoroutine("playAnimation")
            ;
        }
    }

    private IEnumerator playAnimation()
    {

        yield return new WaitForSeconds(1.8f);
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach (GameObject go in gos)
        {
            if ((go.layer == 9) )
            {
                go.SendMessage("DIE");
            }
            if (go.tag == "Finish")
            {
                Destroy(go);
            }
        }
        yield return new WaitForSeconds(1.5f);
        LaunchScenes.Instance.LaunchScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
