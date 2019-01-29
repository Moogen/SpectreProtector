using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Range(0.05f, 0.2f)]
    [SerializeField]
    private float flickerTimer = 0.05f; // Modify the size of the filter every time t = flickerTimer 

    [Range(0.02f, 0.1f)]
    [SerializeField]
    private float sizeIncrement = 0.02f; // The amount we modify the filter by 

    private float timer = 0;
    private bool doFlicker = false;
    private bool increase = true; 

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= flickerTimer)
        {
            doFlicker = !doFlicker;
            timer = timer % flickerTimer; 
        }

        if (doFlicker)
        {
            if (increase)
            {
                transform.localScale += Vector3.one * sizeIncrement;
                increase = !increase;
            }
            else
            {
                transform.localScale -= Vector3.one * sizeIncrement;
                increase = !increase;
            }
            doFlicker = !doFlicker;
        }
    }

    private void OnDrawGizmos()
    {
        // The diameter of the circle is 389 pixels
        // This sprite was imported at 100 pixels per unit
        // Therefore, the radius is 389 / 200 = 1.945
        // However, we also have to take into account the scaling of the object which can change the radius of the light
        float radius = 1.945f;
        radius *= transform.localScale.x; // x and y scale should be the same
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}