using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;



public class IceBlock : MonoBehaviour
{
    [SerializeField] private List<Tilemap> groundTilemaps;
    [SerializeField] private List<TileBase> iceTiles; // Assign your ice tile in the Inspector
    [SerializeField] private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        Vector3Int tilePos = Vector3Int.FloorToInt(groundTilemaps[0].WorldToCell(transform.position));
        bool isOnIce = false;

        foreach (Tilemap tilemap in groundTilemaps)
        {
            TileBase currentTile = tilemap.GetTile(tilePos);
            Debug.Log($"Checking tilemap: {tilemap.name}, Tile at {tilePos}: {(currentTile != null ? currentTile.name : "null")}");

            if (currentTile != null && iceTiles.Contains(currentTile))
            {
                Debug.Log($"ICE TILE DETECTED: {currentTile.name} on tilemap: {tilemap.name}");
                isOnIce = true;
                break;
            }
        }

        player.SetSlippery(isOnIce);
    }
}
