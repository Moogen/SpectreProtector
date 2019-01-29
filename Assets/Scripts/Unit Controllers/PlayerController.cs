using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private Rigidbody2D rb;
    public UnityEvent doAction;
    public UnityEvent undoAction;
    private static PlayerController _instance = null;
    public bool inventoryFilled = false;
    private Animator animator;

    public static PlayerController Instance
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
        // DontDestroyOnLoad(gameObject);

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (doAction != null && !inventoryFilled)
            {
                doAction.Invoke();
            }
            else if (undoAction != null && inventoryFilled)
            {
                undoAction.Invoke();
            }
        }
    }

    private void FixedUpdate() {
        if (inventoryFilled) { speed = 3; }
        else { speed = 4; }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);
        animator.SetBool("MovingDown", false);
        animator.SetBool("MovingUp", false);
        animator.SetBool("MovingLeft", false);
        animator.SetBool("MovingRight", false);
        if (moveHorizontal > 0 && moveHorizontal > Mathf.Abs(moveVertical)) animator.SetBool("MovingRight", true);
        else if (moveHorizontal < 0 && Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical)) animator.SetBool("MovingLeft",true);
        else if (moveVertical > 0 && moveVertical >= Mathf.Abs(moveHorizontal)) animator.SetBool("MovingUp", true);
        else if (moveVertical < 0 && Mathf.Abs(moveVertical) >= Mathf.Abs(moveHorizontal)) animator.SetBool("MovingDown",true);
    }
}