using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float scale = 5f;
    const float vieverMoveTresholdForChunkUpdate = 25f;
    const float sqrvieverMoveTresholdForChunkUpdate = vieverMoveTresholdForChunkUpdate * vieverMoveTresholdForChunkUpdate;
    public static float maxViewDistance;
    public Transform viewer;
    static MapGenerator mapGenerator;
    public static Vector2 viewerPosition;
    Vector2 viewerPositionOld;
    int chunkSize;
    int chunksVisibleInViewDistance;
    public Material mapMaterial;

    public LODInfo[] detailLevels;
   

    Dictionary<Vector2, terrainChunk> terrainChunkDictionary = new Dictionary<Vector2, terrainChunk>();
    static List<terrainChunk> terrainChunksVisibleLastUpdate = new List<terrainChunk>();
    private void Start()
    {
        maxViewDistance = detailLevels[detailLevels.Length-1].visibleDistanceTreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);
        mapGenerator = FindObjectOfType<MapGenerator>();
        UpdateVisibleChunks();
    }
    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;
        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrvieverMoveTresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
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
                    
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new terrainChunk(viewedChunkCoord, chunkSize,detailLevels, transform, mapMaterial));
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
        MeshCollider meshCollider;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        LODMesh collLODmesh;
        MapData mapData;
        bool mapDataRecived;
        int prevLODIndex = -1;
        public terrainChunk(Vector2 coord, int size,LODInfo[] detailLevels, Transform parent, Material material)
        {
            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod,UpdateTerrChunk);
                if (detailLevels[i].useForCollider)
                {
                    collLODmesh = lodMeshes[i];
                }
            }

            this.detailLevels = detailLevels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 posV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("terrain chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshObject.transform.position = posV3 * scale;
            meshObject.transform.localScale = Vector3.one * scale;
            meshRenderer.material = material;
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshObject.transform.parent = parent;
            SetVisible(false);

            mapGenerator.requestMapData(position,OnMapDataRecived);
        }
        void OnMapDataRecived(MapData mapData)
        {

            Texture2D texture = TextureGeneration.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;
            this.mapData = mapData;
            mapDataRecived = true;
            UpdateTerrChunk();
        }
      
        public void UpdateTerrChunk()
        {
            if (mapDataRecived)
            {
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewerDstFromNearestEdge <= maxViewDistance;

                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDistanceTreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (lodIndex != prevLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            prevLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                    if (lodIndex == 0)
                    {
                        if (collLODmesh.hasMesh)
                        {
                            meshCollider.sharedMesh = collLODmesh.mesh;
                        }else if (!collLODmesh.hasRequestedMesh)
                        {
                            collLODmesh.RequestMesh(mapData);
                        }
                    }
                    terrainChunksVisibleLastUpdate.Add(this);
                }
                SetVisible(visible);
            }
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

    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action UpdateCallback;
        public LODMesh(int lod, System.Action UpdateCallback)
        {
            this.lod = lod;
            this.UpdateCallback = UpdateCallback;
        }
        void OnMeshDataRecived(MeshData meshdata)
        {
            mesh = meshdata.createMesh();
            hasMesh = true;

            UpdateCallback();
        }
        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecived);
        }
    }
    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDistanceTreshold;
        public bool useForCollider;
    }
}
