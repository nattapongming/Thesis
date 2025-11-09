using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;
using UnityEngine.UI;
using NavMeshPlus.Components;
using System.Linq;
using UnityEngine.Tilemaps;


public class RoomGeneration : MonoBehaviour
{
    [SerializeField] NavMeshSurface navMeshSurface;

    [Header("Room Settings")]
    public GameObject[] roomPrefabs;  // Array of Tilemap room prefabs (varied widths)
    public int maxRooms = 10;         // Max for initial batch (unique until cycle)
    public Transform startPosition;   // Player spawn/origin (default: this transform)
    public bool bakeNavMeshOnBatch = true;  // Enable/disable batch baking

    [Header("Connection Adjustments")]  // NEW: For asymmetric connects
    public Vector3 leftEntryAdjustment = Vector3.zero;  // e.g., new Vector3(-5f, 0f, 0f) if left connect is offset left of root

    private List<GameObject> generatedRooms = new List<GameObject>();  // Track for management/baking
    private List<int> shuffledOrder;  // Shuffled indices for no early repeats
    private int currentRoomIndex = 0; // For cycling through order in dynamic gen


    void Start()
    {
        if (startPosition == null)
            startPosition = transform;

        // Shuffle prefab order once for variety (no repeats until full cycle)
        ReshuffleOrder();  // UPDATED: Call reshuffle here too for consistency

        // Generate initial batch
        StartCoroutine(GenerateRooms());
        navMeshSurface.BuildNavMesh();
    }

    /// <summary>
    /// Get offset to the single tile in the "connectright" child GameObject's Tilemap.
    /// Finds the child GO, gets its Tilemap, extracts the tile's world pos (rightmost cell).
    /// Returns Vector3(offsetX, offsetY, 0) from room root for snapping.
    /// </summary>
    private Vector3 GetRightConnectOffset(GameObject room)
    {
        Transform roomRoot = room.transform;
        Transform connectGO = roomRoot.Find("connectright");  // Recursive find by name

        if (connectGO == null)
        {
            Debug.LogWarning($"No 'connectright' child found in {room.name}; fallback to Tilemap width");
            return GetFallbackWidth(room);
        }

        Tilemap connectTilemap = connectGO.GetComponent<Tilemap>();
        if (connectTilemap == null)
        {
            Debug.LogWarning($"No Tilemap on 'connectright' in {room.name}; fallback");
            // Fallback to GO position if no Tilemap (previous logic)
            return connectGO.localPosition;
        }

        // Compress to used bounds (essential for single tile)
        connectTilemap.CompressBounds();
        BoundsInt cellBounds = connectTilemap.cellBounds;
        Debug.Log($"Connect Tilemap bounds in {room.name}: {cellBounds} (size: {cellBounds.size})");  // NEW: Debug bounds

        if (cellBounds.size.x == 0 || cellBounds.size.y == 0)
        {
            Debug.LogWarning($"Empty Tilemap in 'connectright' of {room.name}; fallback to GO pos");
            return connectGO.localPosition;
        }

        // NEW: Loop over bounds to find the single non-null tile (robust for any pos)
        Vector3Int foundCellPos = Vector3Int.zero;
        TileBase foundTile = null;
        bool tileFound = false;
        for (int x = cellBounds.xMin; x <= cellBounds.xMax; x++)
        {
            for (int y = cellBounds.yMin; y <= cellBounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase tile = connectTilemap.GetTile(cellPos);
                if (tile != null)
                {
                    foundCellPos = cellPos;
                    foundTile = tile;
                    tileFound = true;
                    break;  // Found it—stop (assumes single tile)
                }
            }
            if (tileFound) break;
        }

        if (!tileFound)
        {
            Debug.LogWarning($"No non-null tile found in 'connectright' Tilemap of {room.name} (bounds: {cellBounds}); fallback to GO pos");
            return connectGO.localPosition;
        }

        // Convert found cell to world pos
        Vector3 tileWorldPos = connectTilemap.CellToWorld(foundCellPos) + connectTilemap.transform.position;
        Debug.Log($"Found connect tile in {room.name}: cell {foundCellPos}, tile '{foundTile.name}', world pos {tileWorldPos}");

        // Offset from room root
        Vector3 offset = tileWorldPos - roomRoot.position;
        Debug.Log($"Final connect offset for {room.name}: {offset}");
        return offset;
    }

    /// <summary>
    /// Fallback: Room-level Tilemap width (unchanged).
    /// </summary>
    private Vector3 GetFallbackWidth(GameObject room)
    {
        Tilemap tilemap = room.GetComponentInChildren<Tilemap>();
        if (tilemap == null)
        {
            return new Vector3(20f, 0f, 0f);
        }

        tilemap.CompressBounds();
        BoundsInt cellBounds = tilemap.cellBounds;
        Vector3 cellSize = tilemap.cellSize;
        float width = cellBounds.size.x * cellSize.x;
        if (width <= 0) width = 20f;
        return new Vector3(width, 0f, 0f);
    }

    /// <summary>
    /// Batch generation: Linear to the right via connect offsets.
    /// </summary>
    IEnumerator GenerateRooms()
    {
        Vector3 currentPos = startPosition.position;
        int roomsGenerated = 0;

        while (roomsGenerated < maxRooms)
        {
            int prefabIndex = shuffledOrder[currentRoomIndex % shuffledOrder.Count];
            GameObject selectedPrefab = roomPrefabs[prefabIndex];
            currentRoomIndex++;

            GameObject newRoom = Instantiate(selectedPrefab, currentPos, Quaternion.identity);
            generatedRooms.Add(newRoom);

            // Get connect offset
            Vector3 rightConnectOffset = GetRightConnectOffset(newRoom);
            Debug.Log($"Generated room {roomsGenerated + 1} (prefab: {selectedPrefab.name}, connect offset: {rightConnectOffset})");

            // Advance position: Snap next root to this connect + left adjustment
            currentPos += rightConnectOffset + leftEntryAdjustment;

            roomsGenerated++;
            yield return null;  // Non-blocking
        }

        if (bakeNavMeshOnBatch)
        {
            yield return StartCoroutine(BakeAllNavMeshes());
        }

        Debug.Log($"Batch generation complete: {maxRooms} rooms, total span ~{currentPos.x - startPosition.position.x}");
    }

    /// <summary>
    /// Public method to generate one more room on demand (e.g., door trigger).
    /// </summary>
    public void GenerateNextRoom()
    {
        StartCoroutine(GenerateSingleRoom());
    }

    /// <summary>
    /// Dynamic single room: Appends to last via connect offset.
    /// </summary>
    private IEnumerator GenerateSingleRoom()
    {
        Vector3 nextPos;
        if (generatedRooms.Count == 0)
        {
            nextPos = startPosition.position;
        }
        else
        {
            GameObject lastRoom = generatedRooms[generatedRooms.Count - 1];
            Vector3 lastRightOffset = GetRightConnectOffset(lastRoom);
            nextPos = lastRoom.transform.position + lastRightOffset + leftEntryAdjustment;
        }

        // Select next prefab (continues shuffled cycle)
        int prefabIndex = shuffledOrder[currentRoomIndex % shuffledOrder.Count];
        GameObject selectedPrefab = roomPrefabs[prefabIndex];
        currentRoomIndex++;

        GameObject newRoom = Instantiate(selectedPrefab, nextPos, Quaternion.identity);
        generatedRooms.Add(newRoom);

        // UPDATED: Use connect offset for log (no more GetRoomWidth)
        Vector3 connectOffset = GetRightConnectOffset(newRoom);
        Debug.Log($"Dynamic room added (prefab: {selectedPrefab.name}, connect offset: {connectOffset}, pos: {nextPos})");

        // Bake only this new room's NavMesh
        yield return StartCoroutine(BakeNavMeshForRoom(newRoom));

    }

    /// <summary>
    /// Bake NavMesh for a single room (NavMesh Plus compatible).
    /// </summary>
    private IEnumerator BakeNavMeshForRoom(GameObject room)
    {
        /*NavMeshSurface surface = room.GetComponentInChildren<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();  // Runtime bake
            Debug.Log($"Baked NavMesh for room: {room.name}");
        }
        else
        {
            Debug.LogWarning($"No NavMeshSurface in room: {room.name}");
        }*/


        yield return null;
    }

    /// <summary>
    /// Bake all generated rooms (for batch mode).
    /// </summary>
    private IEnumerator BakeAllNavMeshes()
    {
        foreach (GameObject room in generatedRooms)
        {
            yield return StartCoroutine(BakeNavMeshForRoom(room));
        }
    }

    /// <summary>
    /// Cleanup: Destroy rooms and reset for fresh gen.
    /// </summary>
    public void ClearRooms()
    {
        foreach (GameObject room in generatedRooms)
        {
            if (room != null)
                Destroy(room);
        }
        generatedRooms.Clear();
        currentRoomIndex = 0;
        ReshuffleOrder();  // NEW: Fresh shuffle on reset for variety
    }

    /// <summary>
    /// Reshuffle prefab order for new runs (no early repeats).
    /// </summary>
    private void ReshuffleOrder()
    {
        shuffledOrder = Enumerable.Range(0, roomPrefabs.Length).ToList();
        shuffledOrder = shuffledOrder.OrderBy(x => Random.value).ToList();
        currentRoomIndex = 0;
    }
}
