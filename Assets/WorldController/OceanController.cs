using UnityEngine;

[RequireComponent(typeof(Plane))]
public class OceanController : MonoBehaviour
{

    public float perlinScale;
    public float waveSpeed;
    public float waveHeight;
    public float offset;

    void Update()
    {
        CalcNoise();
    }

    void CalcNoise()
    {
        MeshFilter mF = GetComponent<MeshFilter>();
        MeshCollider mC = GetComponent<MeshCollider>();

        mC.sharedMesh = mF.mesh;
        mF.sharedMesh.Clear();

        float[,] noiseMap = new float[100, 100];

        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                float pX = (x * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed) + offset;
                float pY = (y * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed) + offset; 

                noiseMap[x, y] = Mathf.PerlinNoise(pX, pY) * waveHeight;
            }
        }


        Vector3[] vertices = TerrainGenerator.GenerateTerrainVertices(noiseMap);
        int[] triangles = TerrainGenerator.GenerateTerrainTriangles(100, 100);
        Vector2[] uv = TerrainGenerator.GenerateTerrainUV(vertices, 100, 100);

        mF.mesh.vertices = vertices;
        mF.mesh.triangles = triangles;
        mF.mesh.uv = uv;
        mF.mesh.RecalculateNormals();
        mF.mesh.RecalculateBounds();
    }

}