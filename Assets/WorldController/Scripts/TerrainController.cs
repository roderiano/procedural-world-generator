using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Threading;

public class TerrainController : MonoBehaviour
{
    [Range(0f, 10f)]
    public float redistribuition;
    [Range(100f, 500f)]
    public int size;
    public int octaves;
    public float scale;
    public Material terrainMaterial;
    public GameObject oceanChunck;

    private float[,] noiseMap;
    
    private Texture2D texture;
    public Dictionary<Vector2, float[,]> instantiatedChuncks = new Dictionary<Vector2, float[,]>();
 
    void Start ()
    {
        
    }

    void Update()
    {
        HandleTerrainChuncks();
    }

    public void HandleTerrainChuncks() 
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
                    
                    if(!instantiatedChuncks.ContainsKey(areaChunck))
                    {
                        instantiatedChuncks.Add(areaChunck, null);
                        UnityMainThread.wkr.AddJob(() => {
                            noiseMap = Noise.GenerateNoiseMap(size, size, scale, octaves, redistribuition, areaChunck);
                            instantiatedChuncks[areaChunck] = noiseMap;
                        }); 

                        UnityMainThread.wkr.AddJob(() => {
                            noiseMap = FallOffGenerator.ApplyFallOffMap(noiseMap, size);
                        }); 

                        UnityMainThread.wkr.AddJob(() => {
                            GameObject terrain = TerrainGenerator.GenerateTerrain(noiseMap, terrainMaterial, areaChunck);
                            GameObject oceanTerrainChunck = GameObject.Instantiate(oceanChunck, terrain.transform.position, terrain.transform.rotation, terrain.transform);
                            oceanTerrainChunck.transform.position = new Vector3(oceanTerrainChunck.transform.position.x + 125f, 4f, oceanTerrainChunck.transform.position.z + 125f);
                        });                              
                    }
                }
            }
        }
    }
}
 