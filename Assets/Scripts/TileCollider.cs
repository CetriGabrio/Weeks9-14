using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCollider : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase stoneTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int tilePosition = tilemap.WorldToCell(transform.position);
        TileBase currentTile = tilemap.GetTile(tilePosition);

        if (currentTile == stoneTile)
        {
            Debug.Log("This is stone");
        }
        else
        {
            Debug.Log("Grass");
        }
    }
}
