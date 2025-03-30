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
            ClearMap();
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
}
