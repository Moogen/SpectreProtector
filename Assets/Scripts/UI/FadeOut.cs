using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public Texture2D fadeOutTexture;
    private int drawDepth = -1000;
    private float alpha = -1.0f, fadeDir = 1, fadeSpeed = 0.3f;

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color col = Color.black;
        col.a = alpha;
        GUI.color = col;
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }
}
