using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowFilter : MonoBehaviour
{
    public Tilemap Floor;
    public Tilemap Filter;

    public Tile FilterTile;

    private void Start()
    {
        Filter.origin = Floor.origin;
        Filter.size = Floor.size; 

        foreach (Vector3Int vec in Filter.cellBounds.allPositionsWithin)
        {
            Filter.SetTile(vec, FilterTile);
        }
    }
}