using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    [Range(0f, 10f)]
    public float redistribuition;
    [Range(100f, 500f)]
    public int size;
    public int octaves;
    public float scale;
    public Material terrainMaterial;
    public GameObject oceanChunck;
    
    private Texture2D texture;
    private Dictionary<Vector2, GameObject> instantiatedChuncks = new Dictionary<Vector2, GameObject>();
 
    void Start ()
    {
        
    }

    void Update()
    {
        HandleTerrainChuncks();
        
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void HandleTerrainChuncks() 
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Vector2 rootChunck = new Vector2((int)(player.transform.position.x / size), (int)(player.transform.position.z / size));

            for(int x = (int)rootChunck.x - 2; x <= (int)rootChunck.x + 2; x++)
            {
                for(int z = (int)rootChunck.y - 2; z <= (int)rootChunck.y + 2; z++)
                {
                    Vector2 areaChunck = new Vector2(rootChunck.x + x, rootChunck.y + z);
                    Debug.Log(areaChunck);
                    
                    if(!instantiatedChuncks.ContainsKey(areaChunck))
                    {
                        float[,] noiseMap = Noise.GenerateNoiseMap(size, size, scale, octaves, redistribuition, areaChunck);
                        noiseMap = FallOffGenerator.ApplyFallOffMap(noiseMap, size);
                        GameObject terrain = TerrainGenerator.GenerateTerrain(noiseMap, terrainMaterial, areaChunck);
                        GameObject oceanTerrainChunck = GameObject.Instantiate(oceanChunck, terrain.transform.position, terrain.transform.rotation, terrain.transform);
                        oceanTerrainChunck.transform.position = new Vector3(oceanTerrainChunck.transform.position.x + 125f, 4f, oceanTerrainChunck.transform.position.z + 125f);
                        instantiatedChuncks.Add(areaChunck, terrain);
                    }
                }
            }

            
        }
    }
}
 