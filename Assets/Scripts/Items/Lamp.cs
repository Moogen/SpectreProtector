using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Item
{
    SpriteRenderer sr; 
    protected override void Start()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
    }
    
    protected override void Act()
    {
        if (!PlayerController.Instance.inventoryFilled)
        {
            transform.SetParent(PlayerController.Instance.transform);
            sr.sortingLayerName = "Item";
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
            PlayerController.Instance.inventoryFilled = false;
            PlayerController.Instance.undoAction.RemoveListener(UnAct);
        }
    }
}