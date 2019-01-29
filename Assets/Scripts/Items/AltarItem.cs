using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarItem : Item
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circCollider;

    #region Monobehaviour Methods 
    protected override void Start()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }    
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        if (circCollider == null)
        {
            circCollider = GetComponent<CircleCollider2D>();
        }
    }
    #endregion

    protected override void Act()
    {
        if (!PlayerController.Instance.inventoryFilled)
        {
            transform.SetParent(PlayerController.Instance.transform);
            sr.sortingLayerName = "Item";
            
            if (boxCollider)
            {
                boxCollider.enabled = false;
            }
            else if (circCollider)
            {
                circCollider.enabled = false;
            }
            
            PlayerController.Instance.inventoryFilled = true;
            PlayerController.Instance.undoAction.AddListener(UnAct);
        }
    }

    protected override void UnAct()
    {
        if (PlayerController.Instance.inventoryFilled)
        {
            transform.SetParent(null);
            sr.sortingLayerName = "Environment";
            
            if (boxCollider)
            {
                boxCollider.enabled = true;
            }
            else if (circCollider)
            {
                circCollider.enabled = true;
            }
            
            PlayerController.Instance.inventoryFilled = false;
            PlayerController.Instance.undoAction.RemoveListener(UnAct);
        }
    }
}