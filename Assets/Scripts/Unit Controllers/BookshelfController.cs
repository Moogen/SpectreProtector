using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BookshelfController : MonoBehaviour
{
    private bool isActive;
    private Animator anmr;
    private int timeWaiting;
    private float rndm;

    private void Start()
    {
        isActive = false;
        rndm = 0;
        anmr = GetComponent<Animator>();
    }
    public void DIE()
    {
        //anmr.SetTrigger("WHAT");
    }

    private void FixedUpdate()
    {

        while (rndm < 200 )
        {
            rndm = Random.value * 1000;
        }

        if (!isActive && timeWaiting>rndm)
        {
            isActive = true;
            anmr.SetTrigger("Activate");
            StartCoroutine("attack");

        }
        else
        {
            timeWaiting++;
        }
        
    }

    private IEnumerator attack()
    {
        yield return new WaitForSeconds(6.4f);
        if (isActive)
        {

            for (int i = 0; i < 10; i++)
            {
                
                if (checkCollisionDirection2(transform.position, new Vector2(0, -1.5f), 1.0f, LayerMask.GetMask("NPC")))
                {
                    LaunchScenes.Instance.LaunchSceneAndStorePrevious("TryAgain");
                }
                if (checkCollisionDirection2(transform.position + new Vector3(-0.5f,0,0), new Vector2(0, -1.5f), 1.0f, LayerMask.GetMask("NPC")))
                {
                    LaunchScenes.Instance.LaunchSceneAndStorePrevious("TryAgain");
                }
                if (checkCollisionDirection2(transform.position + new Vector3(0.5f, 0, 0), new Vector2(0, -1.5f), 1.0f, LayerMask.GetMask("NPC")))
                {
                    LaunchScenes.Instance.LaunchSceneAndStorePrevious("TryAgain");
                }
                yield return new WaitForSeconds(.1f);

            }
            isActive = false;
            timeWaiting = 0;
            rndm = 0;
        }
        
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
        anmr.ResetTrigger("Activate");
        anmr.SetTrigger("Deactive");
        isActive = false;
        rndm = 0;
       
        timeWaiting = 0;
    }
    private bool checkCollisionDirection2(Vector2 origin, Vector2 dir, float distance, int layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layerMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }


}
