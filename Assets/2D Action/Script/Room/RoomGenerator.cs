using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    public GameObject[] roomPrefabs; // Assign your variable-width prefabs here
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;

    public Transform player; // Drag player in Inspector
    public int numRoomsToGenerate = 12; // Total rooms for the level (tune for bullet hell pacing)

    private List<GameObject> activeRooms = new List<GameObject>();
    private float totalLevelWidth = 0f;
    private int seed = 0; // For reproducible randomness (set in Inspector for testing)

    [SerializeField] private NavMeshSurface navMeshSurface;

    void Start()
    {
        // Set seed for consistent testing (e.g., change to Random.Range(0, 1000) for true random)
        if (seed > 0) Random.InitState(seed);

        PreGenerateLevel();
        navMeshSurface.BuildNavMesh();

        // Position player at start of level
        if (player != null) player.position = new Vector3(0f, 0f, 0f);

    }

    void PreGenerateLevel()
    {
        if (roomPrefabs.Length == 0)
        {
            Debug.LogError("No room prefabs assigned!");
            return;
        }

        activeRooms.Clear();
        totalLevelWidth = 0f;
        float currentX = 0f;

        for (int i = 0; i < numRoomsToGenerate; i++)
        {
            // Random selection (add weighting if needed, e.g., boss every 5th: if (i % 5 == 4) pick boss prefab)
            int randomIndex = Random.Range(0, roomPrefabs.Length);

            // Check if it's start or boss room
            GameObject roomObj;
            if (i == 0)
            {
                roomObj = Instantiate(startRoomPrefab, new Vector3(currentX, 0f, 0f), Quaternion.identity);
            } else if (i == numRoomsToGenerate - 1)
            {
                roomObj = Instantiate(bossRoomPrefab, new Vector3(currentX, 0f, 0f), Quaternion.identity);
            } else
            {
                roomObj = Instantiate(roomPrefabs[randomIndex], new Vector3(currentX, 0f, 0f), Quaternion.identity);
            }

            RoomComponent room = roomObj.GetComponent<RoomComponent>();

            if (room == null)
            {
                Debug.LogError($"Prefab at index {randomIndex} missing RoomComponent!");
                continue;
            }

            // Chain to next: Update position for accumulation
            currentX += room.roomWidth;
            totalLevelWidth += room.roomWidth;

            activeRooms.Add(roomObj);

            

            Debug.Log($"Generated room {i + 1}: Width {room.roomWidth}, End X: {currentX}");
        }

        //Debug.Log($"Level pre-generated! Total width: {totalLevelWidth}, {numRoomsToGenerate} rooms.");
    }
}
