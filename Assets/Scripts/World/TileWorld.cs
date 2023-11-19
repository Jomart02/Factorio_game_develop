using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class TileWorld : MonoBehaviour {

        [Serializable] struct TerrainLevel {
            public string name;
            public float minLevel;
            public float maxLevel;
            public GameObject prefab_tile;
        }
        [SerializeField] List<TerrainLevel> terrainLevel = new List<TerrainLevel>();
              
        public GameObject GetTileLevel(float noise ) {
       
            foreach (var level in terrainLevel) {
            Debug.Log(level.name);
                // Диапазон шума
                if (noise > level.minLevel && noise < level.maxLevel) {
                    return level.prefab_tile;
                }
            }
            return terrainLevel[0].prefab_tile;

        }

    }
