using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WandererController : MonoBehaviour
{
    #region Fields
    [SerializeField] private float speed = 2f;
    [SerializeField] private Rigidbody2D rb;
    private bool isWaiting;
    private Animator anmr;

    private List<Vector2> directions = new List<Vector2>
    {
        new Vector2(-1, 0), // Left
        new Vector2(0, 1), // Up
        new Vector2(1, 0), // Right
        new Vector2(0, -1) // Down
    };
    private Vector2 prevDirection;
    private int prevDirIndex; 
    private Dictionary<Vector2, Vector2> oppositeDirections = new Dictionary<Vector2, Vector2>()
    {
        {new Vector2(-1, 0), new Vector2(1, 0)},
        {new Vector2(1, 0), new Vector2(-1, 0)},
        {new Vector2(0, -1), new Vector2(0, 1)},
        {new Vector2(0, 1), new Vector2(0, -1)}
    };
    #endregion

    #region Monobehaviour Methods
    private void Awake()
    {
        anmr = GetComponent<Animator>();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;

        // Initializing the direction randomly 
        float randomNum = Random.value;
        if (randomNum <= 0.25f)
        {
            prevDirIndex = 0;
        }
        else if (randomNum <= 0.50f)
        {
            prevDirIndex = 1;
        }
        else if (randomNum <= 0.75f)
        {
            prevDirIndex = 2;
        }
        else
        {
            prevDirIndex = 3;
        }
        prevDirection = directions[prevDirIndex];
    }
    public void DIE()
    {
        anmr.SetTrigger("WHAT");
    }

    private void FixedUpdate()
    {
        if (!isWaiting)
        {
            List<Vector2> validDirections = getValidDirectionsToMove();
            Vector2 nextMove = getNextDirection(ref validDirections); // This isn't really necessary since the data is contained in prevDirection, but whatever
            StartCoroutine(MoveAction(nextMove));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(prevDirection.x * 1, prevDirection.y * 1));
    }
    #endregion

    #region Methods for Determining the Next Direction 
    /* 
     * This method returns the next direction the Wanderer should head in 
     * How this works depends on how many validDirections there are. In general, the assignment of probabilities discourages our Wanderer from reversing direction on itself. 
     * 
     * If there are 0 valid directions, then we just return (0, 0). i.e., the Wanderer ceases to move 
     * If there is 1 valid direction, then we return that direction with 100% certainty 
     * If there are 2 valid directions, then we check if one of them is the opposite of prevDirection. If it is, then there is a 5% chance to pick that direction, and a 95% chance to pick the other direction. 
     * Otherwise, if neither of them are equal to prevDirection, then they both have a 50% chance of being picked. 
     * If there are 3 valid directions, then we check if one of them is the opposite of prevDirection or if one of them is equal to prevDirection
     * There are 3 cases: only the opposite of prevDirection is present (case a), only prevDirection is present (case b), and both are present (case c). The probabilities are as follows: 
     * a) 5% chance of reversing, 47.5% chance of turning left, and 47.5% chance of turning right
     * b) 50% chance of going forward, 25% chance of turning left, and 25% chance of turning right 
     * c) 5% chance of reversing, 50% chance of going forward, and 45% chance of turning either left or right 
     * If there are 4 valid directions, then there is a 49% chance of moving forward, 25% chance of turning left, 25% chance of turning right, and 1% chance of reversing. 
     */
    private Vector2 getNextDirection(ref List<Vector2> validDirections)
    {
        float randomNum = Random.value;
        switch (validDirections.Count)
        {
            case 0:
                {
                    prevDirection = Vector2.zero;
                    return Vector2.zero;
                }
            case 1:
                {
                    prevDirection = validDirections[0];
                    return validDirections[0];
                }
            case 2:
                {
                    if (validDirections.Contains(oppositeDirections[prevDirection]))
                    {
                        int oppositeDirIndex = validDirections.IndexOf(oppositeDirections[prevDirection]);
                        Vector2 oppositeDirection = validDirections[oppositeDirIndex];
                        if (randomNum >= 0.95f)
                        {
                            prevDirection = oppositeDirection;
                            prevDirIndex = (prevDirIndex + 2) % 4;
                            return oppositeDirection;
                        }
                        else
                        {
                            prevDirection = validDirections[(oppositeDirIndex + 1) % 2];
                            prevDirIndex = directions.IndexOf(prevDirection);
                            return validDirections[(oppositeDirIndex + 1) % 2];
                        }
                    }
                    else
                    {
                        if (randomNum >= 0.5f)
                        {
                            prevDirection = validDirections[0];
                            prevDirIndex = directions.IndexOf(prevDirection);
                            return validDirections[0];
                        }
                        else
                        {
                            prevDirection = validDirections[1];
                            prevDirIndex = directions.IndexOf(prevDirection);
                            return validDirections[1];
                        }
                    }
                }
            case 3:
                {
                    int reverse = validDirections.IndexOf(oppositeDirections[prevDirection]);
                    int forward = validDirections.IndexOf(prevDirection);
                    if (reverse == -1 && forward != -1) // Case a
                    {
                        if (randomNum >= 0.95f) // Reversing directions 
                        {
                            prevDirIndex = (prevDirIndex + 2) % 4;
                            prevDirection = directions[prevDirIndex];
                        }
                        else if (randomNum >= 0.475f) // Turning left
                        {
                            prevDirIndex = (prevDirIndex + 3) % 4;
                            prevDirection = directions[prevDirIndex];
                        }
                        else // Turning right 
                        {
                            prevDirIndex = (prevDirIndex + 1) % 4;
                            prevDirection = directions[prevDirIndex];
                        }
                    }
                    else if (forward == -1 && reverse != -1) // Case b
                    {
                        if (randomNum >= 0.5f) // Going forward
                        {
                            // Nothing changed
                        }
                        else if (randomNum >= 0.25f) // Turning left 
                        {
                            prevDirIndex = (prevDirIndex + 3) % 4;
                            prevDirection = directions[prevDirIndex];
                        }
                        else // Turning right 
                        {
                            prevDirIndex = (prevDirIndex + 1) % 4;
                            prevDirection = directions[prevDirIndex];
                        }
                    }
                    else // Case c
                    {
                        if (randomNum >= 0.95f) // Reversing directions
                        {
                            prevDirIndex = (prevDirIndex + 2) % 4;
                            prevDirection = directions[prevDirIndex];
                        }
                        else if (randomNum >= 0.45f) // Going forward 
                        {
                            // Nothing changed 
                        }
                        else
                        {
                            float randomNum2 = Random.value;
                            if (randomNum2 >= 0.5f) // Turning Left
                            {
                                prevDirIndex = (prevDirIndex + 3) % 4;
                                prevDirection = directions[prevDirIndex];
                            }
                            else // Turning right
                            {
                                prevDirIndex = (prevDirIndex + 1) % 4;
                                prevDirection = directions[prevDirIndex];
                            }
                        }
                    }
                    return prevDirection;
                }
            default:
                {
                    if (randomNum >= 0.99f) // Reversing directions 
                    {
                        prevDirIndex = (prevDirIndex + 2) % 4; 
                        prevDirection = directions[prevDirIndex];
                    }
                    else if (randomNum >= 0.74f) // Turning left
                    {
                        prevDirIndex = (prevDirIndex + 3) % 4;
                        prevDirection = directions[prevDirIndex];
                    }
                    else if (randomNum >= 0.49f) // Turning right 
                    {
                        prevDirIndex = (prevDirIndex + 1) % 4;
                        prevDirection = directions[prevDirIndex];
                    }
                    else // Going straight
                    {
                        // Do nothing 
                    }
                    return prevDirection;
                }
        }
    }

    /*
     * This method determines which directions are still valid for the Wanderer to move in. 
     * The Wanderer may be inhibited by walls and sources of light (currently just lamps). 
     */
    private List<Vector2> getValidDirectionsToMove()
    {
        List<Vector2> validDirections = new List<Vector2>() { new Vector2(-1, 0),
                                                              new Vector2(0, 1),
                                                              new Vector2(1, 0),
                                                              new Vector2(0, -1)};

        detectLight(ref validDirections);
        // Do something with walls
        return validDirections;
    }

    /*
     * This method detects if the Wanderer is within the area of effect range of a Lamp. Lamps have a light radius of 1.945 units, but have an area of effect of 2.35 units
     * It returns a list of valid directions to go. Directions are represented as Vector2 objects: left is (-1, 0), up is (0, 1), etc. 
     */
    private void detectLight(ref List<Vector2> validDirections)
    {
        float radius = 2.2f;
        List<Vector2> lampPositions = new List<Vector2>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius, LayerMask.GetMask("Item"));
        foreach (Collider2D col in hits)
        {
            if (col.gameObject.GetComponentInChildren<Lamp>()) // This tests if there is a lamp script on any objects within range 
            {
                lampPositions.Add(new Vector2(col.transform.position.x, col.transform.position.y));
            }
        }

        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        foreach (Vector2 lampPos in lampPositions)
        {
            if ((currentPos - lampPos).magnitude <= radius) // Wanderer is within the range of effect of the lamp - note that the radius of the lamp is 1.945. We want him to avoid it even if he is outside of its area of effect
            {
                List<Vector2> badDirections = new List<Vector2>();
                // Determine which directions are cut off
                foreach (Vector2 dir in validDirections)
                {
                    Vector2 nextPos = currentPos + dir;
                    if ((nextPos - lampPos).magnitude <= radius)
                    {
                        badDirections.Add(dir);
                    }
                }
                foreach (Vector2 badDir in badDirections)
                {
                    validDirections.Remove(badDir);
                }
                if (validDirections.Count == 0)
                {
                    break;
                }
            }
        }
    }
    #endregion

    #region Move the Wanderer
    private IEnumerator MoveAction(Vector2 directions)
    {
        if (Mathf.Abs(directions.x) > 0 || Mathf.Abs(directions.y) > 0)
        {
            string[] collisions = new string[2];
            collisions[0] = "Outer Wall";
            collisions[1] = "Inner Wall";
            if (!checkCollisionDirection(transform.position, directions, 1.0f, LayerMask.GetMask(collisions)))
            {
                isWaiting = true;
                yield return StartCoroutine(Move(transform, transform.position, transform.position + new Vector3(directions.x, directions.y, 0), speed));
            }
            if (checkCollisionDirection(transform.position, directions, 1.0f, LayerMask.GetMask("NPC")))
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

    private bool checkCollisionDirection(Vector2 origin, Vector2 dir, float distance, int layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layerMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
    #endregion
}