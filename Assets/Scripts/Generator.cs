using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    [Header("World Settings")]
    public int sizeX = 50;
    public int sizeY = 50;
    public int sizeZ = 50;
    public float seed = 0;

    [Header("Noise Settings")]
    public float noiseScale = 25f;
    
    [Header("Tiles")]
    public GameObject groundTile;

    private float[,] grid;
    void Start()
    {
        grid = new float[sizeX, sizeZ];
        
        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-10000, 10000);
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var px = (x+seed) / noiseScale;
                var pz = (z+seed) / noiseScale;
                
                grid[x,z] = (noise.snoise(new float2(px,pz)) + 1) / 2;
            }
        }
    }

    void BuildWorld()
    {
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var height = (int)Mathf.Ceil(grid[x,z] * sizeY);

                for (int y = height; y >= 0; y--)
                {
                    Instantiate(groundTile, new Vector3(x,height,z), Quaternion.identity);
                }
                Instantiate(groundTile, new Vector3(x, height-1, z), Quaternion.identity); // spawn one more layer for ground thickness
            }
        }
    }
}
