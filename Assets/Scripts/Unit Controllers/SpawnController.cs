using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    //[SerializeField] private float speed = 0.2f;
    [SerializeField] private Rigidbody2D rb1;
    [SerializeField] private Transform location1;
    private bool isWaiting;
    private int prevDirection;
    private Animator anmr;
    private AudioSource src;

    //instead of defining an enum of movedirections, we'll basically just use a base 4 rotator
    /*
     * 0 - up
     * 1 - right
     * 2 - down
     * 3 - left
     */

    void Start()
    {
        if (rb1 == null)
        {
            rb1 = GetComponent<Rigidbody2D>();
        }
        rb1.gravityScale = 0;
        
        if (location1 == null)
        {
            location1 = GetComponent<Transform>();
        }
        prevDirection = 0;
        anmr = GetComponent<Animator>();
        src = GetComponent<AudioSource>();

    }

    public void DIE()
    {
        anmr.SetTrigger("WHAT");
    }

    private void FixedUpdate()
    {
        if (!isWaiting)
        {

            Vector2 theMove = getNextDirection();
            StartCoroutine("MoveAction", theMove);
            
        }
    }

    private Vector2 getNextDirection()
    {
        float RandomNumber = Random.value;
        if (RandomNumber >= 0.99f) //move opposite direction
        {
            prevDirection = (prevDirection + 2) % 4;
        }
        else if (RandomNumber >= 0.74f) //turn left
        {
            prevDirection = (prevDirection -1) % 4;
        }
        else if (RandomNumber >= 0.49f) //turn right
        {
            prevDirection = (prevDirection + 1) % 4;
        }
        else //go straight
        {
            //no need to change previous direction
        }

        switch (prevDirection)
        {
            case 0: //up
                {
                    return new Vector2(0, 1);
                }
            case 1: //right
                {
                    return new Vector2(1, 0);
                }
            case 2: //down
                {
                    return new Vector2(0, -1);
                }
            default: //left
                {
                    return new Vector2(-1, 0);
                }
        }
    }

    private IEnumerator MoveAction(Vector2 directions)
    {
        if (Mathf.Abs(directions.x) > 0 || Mathf.Abs(directions.y) > 0)
        {
            string[] collisions = new string[2];
            collisions[0] = "Outer Wall";
            collisions[1] = "Inner Wall";
            //collisions[2] = "Enemy";
            if (!checkCollisionDirection2(transform.position, directions, 0.9f, LayerMask.GetMask(collisions)))
            {
                isWaiting = true;
                yield return StartCoroutine(Move(transform, transform.position, transform.position + new Vector3(directions.x, directions.y, 0), 2));
                // transform.SetPositionAndRotation(new Vector3(transform.localPosition.x + directions.x, transform.localPosition.y + directions.y, 0), Quaternion.identity);
                //yield return new WaitForSeconds(0.1f);
            }
            if (checkCollisionDirection2(transform.position, directions, 1.0f, LayerMask.GetMask("NPC")))
            {
                LaunchScenes.Instance.LaunchSceneAndStorePrevious("TryAgain");
            }
        }
            isWaiting = false;
        
    }

    private IEnumerator Move(Transform obj, Vector3 startPos, Vector3 endPos, float speed)
    {
        float step = (speed / (startPos - endPos).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step;
            obj.position = Vector3.Lerp(startPos, endPos, t);
            yield return new WaitForFixedUpdate();
        }
        obj.position = endPos;
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
        isWaiting = true;
        anmr.SetTrigger("Ded");
        StartCoroutine("playAnimation");
        src.Play();
    }

    private IEnumerator playAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(this.gameObject);

    }

}