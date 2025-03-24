using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Mathematics;

public class Generator3D : MonoBehaviour
{
    [Header("World Settings")]
    public int sizeX = 50;
    public int sizeY = 50;
    public int sizeZ = 50;
    public int seed = 0;
    [Range(0,1)]public float groundLimit = 0.5f;
    
    [Header("Noise Settings")]
    public float noiseScale = 25;
    
    [Header("Tiles")]
    public GameObject groundTile;

    private float[,,] grid;
    
    void Start()
    {
        grid = new float[sizeX, sizeY, sizeZ];
        
        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        for (int z = 0; z < sizeZ; z++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    var px = (x+seed) / noiseScale;
                    var py = (y+seed) / noiseScale;
                    var pz = (z+seed) / noiseScale;
                    
                    grid[x, y, z] = (noise.snoise(new float3(px, py, pz))+1)/2;
                }
            }
        }
    }

    void BuildWorld()
    {
        for (int z = 0; z < sizeZ; z++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (grid[x, y, z] >= groundLimit && IsExposed(x, y, z))
                    {
                        Instantiate(groundTile, new Vector3(x,y,z), Quaternion.identity);
                    }
                }
            }
        }
    }

    bool IsExposed(int x, int y, int z)
    {
        int[,] directions =
        {
            { 1, 0, 0 }, { -1, 0, 0 }, // left right
            { 0, 1, 0 }, { 0, -1, 0 }, // up down
            { 0, 0, 1 }, { 0, 0, -1 } // forward backward
        };

        for (int i = 0; i < 6; i++)
        {
            var nx = x + directions[i, 0];
            var ny = y + directions[i, 1];
            var nz = z + directions[i, 2];

            if (nx < 0 || nx >= sizeX ||
                ny < 0 || ny >= sizeY ||
                nz < 0 || nz >= sizeZ)
            {
                return true; // exposed to boundary
            }

            if (grid[nx, ny, nz] < groundLimit) return true; // has at least one open neighbour
        }

        return false; // fully closed
    }
}
