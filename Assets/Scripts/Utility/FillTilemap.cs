using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FillTilemap : MonoBehaviour
{
    public Tile FillTile;
    public Tilemap ToFill;
  
	void Start ()
    {

        foreach(Vector3Int point in ToFill.cellBounds.allPositionsWithin)
        {
            ToFill.SetTile(point, FillTile);
        }
	}
}
