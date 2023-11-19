using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridGenerator : MonoBehaviour {

    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_plains;
    public GameObject prefab_forest;
    public GameObject prefab_black;
    public GameObject prefab_green;
    public GameObject prefab_green2;
    public GameObject prefab_green3;

    public int gridHeight = 10;
    public int gridWidth = 10;
    public float tileSize = 1f;

/*    public string seed;
    public bool useRandomSeed;*/

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    // recommend 4 to 20
    [Range(0, 50)]
    public float magnification = 7.0f;

    public int x_offset = -10; // <- +>
    public int y_offset = -10; // v- +^


    // Start is called before the first frame update
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }


    void CreateTileset() {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/

        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_plains);
        tileset.Add(1, prefab_forest);
        tileset.Add(2, prefab_black);
        tileset.Add(3, prefab_green);
        tileset.Add(4, prefab_green2);
        tileset.Add(5, prefab_green3);

    }


    void CreateTileGroups() {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/

        tile_groups = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> prefab_pair in tileset) {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }


    void GenerateMap() {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/

        for (int x = 0; x < gridWidth; x++) {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for (int y = 0; y < gridHeight; y++) {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y) {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/

        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification
        );
        float clamp_perlin = Mathf.Clamp01(raw_perlin); // Thanks: youtu.be/qNZ-0-7WuS8&lc=UgyoLWkYZxyp1nNc4f94AaABAg
        float scaled_perlin = clamp_perlin * tileset.Count;

        // Replaced 4 with tileset.Count to make adding tiles easier
        if (scaled_perlin == tileset.Count) {
            scaled_perlin = (tileset.Count - 1);
        }
        return Mathf.FloorToInt(scaled_perlin);
    }

    void CreateTile(int tile_id, int x, int y) {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tile_grid[x].Add(tile);
    }
}






// Update is called once per frame
/*private void GenerateGrid() { 
        for(int x =0; x<gridWidth; x++) {
            for (int y =0; y < gridHeight; y++) {

                var randomTile = tileMap.tileset[1];

                GameObject newTile = Instantiate(randomTile, transform);
                
                //Izometric variant 
                //float posX = (x * tileSize + y * tileSize) / 2f;
                //float posY = (x * tileSize - y * tileSize) / 4f;

                newTile.transform.position = new Vector2(x, y);
                newTile.name = "Tile" + x + "," + y;
            }        
        }
    }

}*/


