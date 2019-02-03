using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarPlinth : Item
{
    private SpriteRenderer sr; 
    [SerializeField] private Sprite usedImage;
    private SpriteMask lightMask;
    private bool isUsed = false;

    protected override void Start()
    {   
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        if (lightMask == null)
        {
            lightMask = GetComponentInChildren<SpriteMask>();
        }
        lightMask.enabled = false;
    }

    protected override void Update()
    {
        if (!isUsed)
        {
            Collider2D collision = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 0.25f, LayerMask.GetMask("Item"));
            if (collision != null && collision.gameObject.tag == "Altar Item")
            {
                isUsed = true;
                sr.sprite = usedImage;
                lightMask.enabled = true;
                Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                ReceiveWin.Instance.getAWin();
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't need to add the Act delegate
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        // Don't need to remove the Act delegate
    }

    protected override void Act()
    {
        // The fact that these methods don't have anything in them makes me want to scrap the Item inheritance in favor of interfaces
    }

    protected override void UnAct()
    {
    }
}