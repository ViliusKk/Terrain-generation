using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Generator2D : MonoBehaviour
{
    [Header("World Settings")]
    public int width = 100;
    public int length = 100;
    [Range(0, 1)]public float groundLimit = 0.5f;
    public float seed;

    [Header("Tiles")] 
    public GameObject groundTile;
    public GameObject[] decorationTiles;

    [Header("Noise Settings")] 
    public float scale = 100f;
    
    private float[,] grid;

    private void Start()
    {
        grid = new float[width, length];
        GenerateNoise();
        BuildWorld();
        Decorate();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                var px = (x + seed) / scale;
                var pz = (z + seed) / scale;
                
                grid[x, z] = noise.snoise(new float2(px, pz)) + 1 / 2;
            }
        }
    }

    void BuildWorld()
    {
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, z] >= groundLimit)
                {
                    Instantiate(groundTile, new Vector3(x, 0, z), Quaternion.identity);
                }
            }
        }
    }

    public void Decorate()
    {
        for (int i = 0; i < 200; i++)
        {
            var x = Random.Range(0, width);
            var z = Random.Range(0, length);
            var ray = new Ray(new Vector3(x, 10, z), Vector3.down);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var tile = decorationTiles[Random.Range(0, decorationTiles.Length)];
                
                var rotationY = Random.Range(0f, 360f);
                
                Instantiate(tile, hit.point, Quaternion.Euler(0, rotationY, 0));
            }
        }
    }
}
