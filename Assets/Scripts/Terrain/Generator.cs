using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class Generator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verticis;
    int[] triangles;

    public int xChunkSize = 16;
    public int zChunkSize = 16;

    public float scale = 7;

    public float offsetX = 1f;
    public float offsetZ = 1f;

    public int renderDistance = 8;


    private void Start()
    {

        offsetX = Random.Range(0f, 99999f);
        offsetZ = Random.Range(0f, 99999f);
        for (int rX = 0; rX < renderDistance; rX++)
        {
            for (int rZ = 0; rZ < renderDistance; rZ++)
            {
                generateChunck(rX, rZ);
                UpdateMesh(rX, rZ);
            }
        }
    }
   
    float GenerateNoise(int x, int z)
    {
        float xcoord = (float)x / xChunkSize * scale + offsetX ;
        float zcoord = (float)z / zChunkSize * scale + offsetZ ;

        float noise = Mathf.PerlinNoise(xcoord, zcoord);
        return noise;
    }

    void generateChunck(int rX, int rZ)
    {
        verticis = new Vector3[(xChunkSize + 1) * (zChunkSize + 1)];
        for (int i = 0, z = 0; z <= zChunkSize; z++)
        {
            for (int x = 0; x <= xChunkSize; x++)
            {
                float y = GenerateNoise(x, z);
                verticis[i] = new Vector3(x , y, z );
                i++;
            }
        }
        int vert = 0;
        int tris = 0;
        triangles = new int[xChunkSize * zChunkSize * 6];
        for (int z = 0; z < zChunkSize; z++)
        {
            for (int x = 0; x < xChunkSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xChunkSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xChunkSize + 1;
                triangles[tris + 5] = vert + xChunkSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    void UpdateMesh(int rX, int rZ)
    {

        GameObject chunk = new GameObject();
        chunk.transform.SetParent(gameObject.transform);
        chunk.AddComponent<MeshRenderer>();
        chunk.AddComponent<MeshFilter>();
        chunk.transform.position = new Vector3(xChunkSize*rX,0,zChunkSize*rZ);
        chunk.GetComponent<MeshRenderer>().material.color = Color.red;
        mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = verticis;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        chunk.GetComponent<MeshFilter>().mesh = mesh;
    }
}


