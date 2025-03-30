using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class TerrainShaper : MonoBehaviour
{
    public float seed;
    public float scale = 100;
    public float octaves = 4;
    
    private Terrain terrain;
    private TerrainData terrainData;
    private int resolution;
    private float[,] map;
    private float[,,] textureMap;
    
    private const int ROCK = 0;
    private const int SAND = 1;
    private const int GRASS = 2;
    
    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        resolution = terrainData.heightmapResolution;
        map = new float[resolution, resolution];
        
        ShapeTerrain();
        PaintTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearMap();
            ShapeTerrain();
            PaintTerrain();
        }
    }

    void ShapeTerrain()
    {
        seed = Random.Range(-100000, 100000);
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                for (int o = 0; o < octaves; o++)
                {
                    var px = (x + seed) / scale * Mathf.Pow(2, o);
                    var pz = (z + seed) / scale * Mathf.Pow(2, o);
                    
                    var noiseValue = (noise.snoise(new float2(px, pz)) + 1) / 2 / Mathf.Pow(2, o);
                    
                    var sign = o % 2 == 0 ? 1 : -1;
                    
                    map[x, z] += noiseValue * sign;
                }
            }
        }
        
        terrainData.SetHeights(0, 0, map);
        terrain.Flush();
    }

    void ClearMap()
    {
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                map[x, z] = 0;
            }
        }
    }

    void PaintTerrain()
    {
        var height = terrainData.alphamapHeight;
        var width = terrainData.alphamapWidth;
        
        textureMap = terrainData.GetAlphamaps(0, 0, width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // clear
                textureMap[x, y, ROCK] = 0;
                textureMap[x, y, SAND] = 0;
                textureMap[x, y, GRASS] = 0;
                
                // apply texture by height
                if(map[x, y] < 0.2f) textureMap[x, y, SAND] = 1;
                else if (map[x, y] < 0.4f) textureMap[x, y, GRASS] = 1;
                else textureMap[x, y, ROCK] = 1;
            }
        }
        
        terrainData.SetAlphamaps(0, 0, textureMap);
        terrain.Flush();
    }
}
