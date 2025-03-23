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

    [Header("Tiles")] 
    public GameObject groundTile;
    
    private float[,] grid;

    private void Start()
    {
        grid = new float[width, length];
        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, z] = noise.snoise(new float2(x, z)) + 1 / 2;
                print(grid[x, z]);
            }
        }
    }

    void BuildWorld()
    {
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Instantiate(groundTile, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }
}
