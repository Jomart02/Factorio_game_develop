using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour {

    [SerializeField] GameObject[] tile;
    [SerializeField] int gridHeight = 10;
    [SerializeField] int gridWidth = 10;
    [SerializeField] float tileSize = 1f;



    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    private void GenerateGrid() { 
        for(int x =0; x<gridWidth; x++) {
            for (int y =0; y < gridHeight; y++) {

                var randomTile = tile[Random.Range(0, tile.Length)];

                GameObject newTile = Instantiate(randomTile, transform);
                float posX = (x * tileSize + y * tileSize) / 2f;
                float posY = (x * tileSize - y * tileSize) / 4f;

                newTile.transform.position = new Vector2(posX, posY);
                newTile.name = "Tile" + x + "," + y;
            }        }
    }
}


