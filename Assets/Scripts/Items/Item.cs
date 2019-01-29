using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected abstract void Act();
    protected abstract void UnAct();
    protected virtual void Start() { }
    protected virtual void Update() { }

    protected bool added = false;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hi");
        if (collision.gameObject.tag == "Player" && !added)
        {
            PlayerController.Instance.doAction.AddListener(Act);
            added = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Bye");
        if (collision.gameObject.tag == "Player" && added)
        {
            PlayerController.Instance.doAction.RemoveListener(Act);
            added = false;
        }
    }
}