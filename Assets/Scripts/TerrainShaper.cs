using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class TerrainShaper : MonoBehaviour
{
    public float seed;
    public float scale = 100;
    
    private Terrain terrain;
    private TerrainData terrainData;
    private int resolution;
    private float[,] map;
    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        resolution = terrainData.heightmapResolution;
        map = new float[resolution, resolution];
        
        ShapeTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShapeTerrain();
        }
    }

    void ShapeTerrain()
    {
        seed = Random.Range(-100000, 100000);
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                var px = (x + seed) / scale;
                var pz = (z + seed) / scale;
                map[x, z] = (noise.snoise(new float2(px, pz)) + 1) / 2;
            }
        }
        
        terrainData.SetHeights(0, 0, map);
        terrain.Flush();
    }
}
