using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDistance = 450;
    public Transform viewer;
    static MapGenerator mapGenerator;
    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksVisibleInViewDistance;
    public Material mapMaterial;

    Dictionary<Vector2, terrainChunk> terrainChunkDictionary = new Dictionary<Vector2, terrainChunk>();
    List<terrainChunk> terrainChunksVisibleLastUpdate = new List<terrainChunk>();
    private void Start()
    {
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);
        mapGenerator = FindObjectOfType<MapGenerator>();
    }
    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }
    
    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++)
        {
            for (int XOffset = -chunksVisibleInViewDistance; XOffset <= chunksVisibleInViewDistance; XOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + XOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrChunk();
                    if (terrainChunkDictionary[viewedChunkCoord].isVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new terrainChunk(viewedChunkCoord,chunkSize,transform, mapMaterial));
                }
            }
        }
    }
    public class terrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        public terrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 posV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("terrain chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshObject.transform.position = posV3;
            meshRenderer.material = material;
            meshObject.transform.parent = parent;
            SetVisible(false);

            mapGenerator.requestMapData(OnMapDataRecived);
        }
        void OnMapDataRecived(MapData mapData)
        {
            mapGenerator.RequestMeshData(mapData, OnMeshDataRecived);
        }
        void OnMeshDataRecived(MeshData meshData)
        {
            meshFilter.mesh = meshData.createMesh();
        }
        public void UpdateTerrChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }
        public bool isVisible()
        {
            return meshObject.activeSelf;
        }
    }
}
