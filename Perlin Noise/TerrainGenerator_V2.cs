using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator_V2.cs : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public int depth = 20;
    public float scale = 20f;

    public bool useRandomSeed = true;
    public string seed;

    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        float seedX = useRandomSeed ? Random.Range(0f, 100f) : GetSeedX();
        float seedY = useRandomSeed ? Random.Range(0f, 100f) : GetSeedY();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale + seedX;
                float yCoord = (float)y / height * scale + seedY;

                heights[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        return heights;
    }

    float GetSeedX()
    {
        return string.IsNullOrEmpty(seed) ? 0f : seed.GetHashCode();
    }

    float GetSeedY()
    {
        return string.IsNullOrEmpty(seed) ? 0f : seed.GetHashCode() + 1f;
    }
}
