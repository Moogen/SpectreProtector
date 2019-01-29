using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    private int spawnCounter;

    private int spawnTimer;
    private Animator anmr;
    public GameObject prefab;
    private bool isWaiting;
    private int timeToWait;
    private bool loading;
    private AudioSource src;
    private AudioClip stunSound;


    void Start()
    {
        spawnTimer = 0;
        spawnCounter = 0;
        anmr = GetComponent<Animator>();
        timeToWait = 60;
        src = GetComponent<AudioSource>();
        stunSound = Resources.Load<AudioClip>("stun4");

    }

    public void DIE()
    {
        anmr.SetTrigger("WHAT");   
    }

    private void FixedUpdate()
    {
        if (spawnTimer > timeToWait)
        {
            float RandomNumber = UnityEngine.Random.value;
            if (RandomNumber * 100 - spawnCounter < 50)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    anmr.SetTrigger("SpawnTrigger");
                    src.Play();
                    StartCoroutine("SpawnGuy");
                    
                }

            }
        }
        else if (!loading)
        {
            spawnTimer++;
        }
    }

    private IEnumerator SpawnGuy()
    {
        //prefab

        yield return new WaitForSeconds(0.5f);
        GameObject blob = Instantiate(prefab, GetComponent<Transform>().position + Vector3.right, Quaternion.identity) as GameObject;
        WandererController jeffIsCuteDargon = blob.GetComponent<WandererController>();
        blob.transform.parent = transform;
        // Instantiate(prefab, GetComponent<Transform>().position+ new Vector3(1,0,0),Quaternion.identity);
        spawnCounter++;
        spawnTimer = 0;
        isWaiting = false;
        yield return new WaitForSeconds(0.25f);
        src.Pause();
        yield return null;

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Item Encountered");
        if (collision.gameObject.name == "PlayerCharacter")
        {
            PlayerController.Instance.doAction.AddListener(Act);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Goodbye");
        if (collision.gameObject.name == "PlayerCharacter")
        {
            PlayerController.Instance.doAction.RemoveListener(Act);

        }
    }

    protected void Act()
    {
        if (!loading)
        {
            src.PlayOneShot(stunSound);
            timeToWait = 100;
            anmr.SetTrigger("Die");
            StartCoroutine("wait");
            loading = true;
            spawnTimer = 0;
        }


    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine("waitmore");
        
    }
    private IEnumerator waitmore()
    {
        anmr.SetTrigger("Respawn");
        yield return new WaitForSeconds(0.55f);
        StartCoroutine("waitmost");
    }
    private IEnumerator waitmost()
    {
        yield return new WaitForSeconds(1.55f);
        anmr.SetTrigger("Continue");
        loading = false;
    }

}
