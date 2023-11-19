using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;



    public class PerlinGenerate : MonoBehaviour {

        [SerializeField] int gridHeight = 10;
        [SerializeField] int gridWidth = 10;
        [SerializeField] float scale = 30f; // �������� �� ��������� � ������������������ ����� 
        [SerializeField] int octaves;
        [SerializeField] float persistence;
        [SerializeField] float lacunarity;
        [SerializeField] Vector2 offset;

        int seed; //��������������� ��������, ������������ ��� �������� �������� ����, ����� ��� ��������� ��-������� ������ ��� ��� ��� ��������.

        TileWorld tileWorld;

        // Start is called before the first frame update
        void Start() {
            tileWorld = GetComponent<TileWorld>();// ��������� ������� ������
            GenerateWorld();
        }

        // Update is called once per frame
        void Update() {

        }

        void GenerateWorld() {

            seed = UnityEngine.Random.Range(0, 9999999);
            System.Random rand = new System.Random(seed);

            // ����� �����, ����� ��� ��������� ���� �� ����� �������� ����� ���������� ��������
            Vector2[] octavesOffset = new Vector2[octaves];
            for (int i = 0; i < octaves; i++) {
                // ��������� ������� ����� ���������
                float xOffset = rand.Next(-100000, 100000) + offset.x;
                float yOffset = rand.Next(-100000, 100000) + offset.y;
                octavesOffset[i] = new Vector2(xOffset / gridWidth, yOffset / gridHeight);
            }

            // ��������� �������� ������ � ������, ��� ����� ��������� ��������� ��������� ��������
            float halfWidth = gridWidth / 2f;
            float halfHeight = gridHeight / 2f;

            for (int x = 0; x < gridWidth; x++) {
                    for (int y = 0; y < gridHeight; y++) {

                        // ����� �������� ��� ������ ������
                        float amplitude = 1;
                        float frequency = 1; //�������������� �������������� ��������
                        float noiseHeight = 0;
                        float superpositionCompensation = 0;

                        // ��������� ��������� �����
                        for (int i = 0; i < octaves; i++) {
                            // ������������ ���������� ��� ��������� �������� �� ���� �������
                            float xResult = (x - halfWidth) / scale * frequency + octavesOffset[i].x * frequency;
                            float yResult = (y - halfHeight) / scale * frequency + octavesOffset[i].y * frequency;

                            // ��������� ������ �� ����
                            float generatedValue = Mathf.PerlinNoise(xResult, yResult);
                            // ��������� �����
                            noiseHeight += generatedValue * amplitude;
                            // ������������ ��������� �����, ����� �������� � �������� ��������� [0,1]
                            noiseHeight -= superpositionCompensation;

                            // ������ ���������, ������� � ����������� ��� ��������� ������
                            amplitude *= persistence;
                            frequency *= lacunarity;
                            superpositionCompensation = amplitude / 2;
                        }


                    // noise = Mathf.PerlinNoise((i + sid) / zoom, (j + sid) / zoom);



                        GenerateTile(x, y, noiseHeight);

                    
                    }
            }
        }


        void GenerateTile(int i, int j, float noise) {

            GameObject tileType = tileWorld.GetTileLevel(noise);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            GameObject tile = Instantiate(tileType, new Vector2(i, j), rot);
            tile.name = string.Format("tile_n{0}", noise);
        }
    }


