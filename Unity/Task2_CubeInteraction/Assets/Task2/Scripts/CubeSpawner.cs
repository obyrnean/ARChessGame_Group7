using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class CubeSpawner : MonoBehaviour
{
    public GameObject chessBoardPrefab;
    public GameObject[] chessPiecePrefabs;
    public float editorSpawnDistance = 1.5f;

    private ARRaycastManager raycastManager;
    private GameObject spawnedBoard;
    private List<GameObject> spawnedPieces = new List<GameObject>();
    private List<Vector3> defaultPositions = new List<Vector3>();
    private bool boardSpawned = false;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (boardSpawned) return;

        bool tapped = false;

#if UNITY_EDITOR
        tapped = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
#else
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            tapped = Touchscreen.current.touches[0].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began;
#endif

        if (!tapped) return;

        bool spawnedViaAR = false;

#if !UNITY_EDITOR
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        Vector2 screenPos = Touchscreen.current != null
            ? Touchscreen.current.touches[0].position.ReadValue()
            : new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (raycastManager != null && raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            SpawnBoard(hitPose.position, hitPose.rotation);
            spawnedViaAR = true;
        }
#endif

        if (!spawnedViaAR)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 spawnPos = cam.transform.position + cam.transform.forward * editorSpawnDistance;
                spawnPos.y = cam.transform.position.y - 0.3f;
                SpawnBoard(spawnPos, Quaternion.identity);
            }
        }
    }

    void SpawnBoard(Vector3 position, Quaternion rotation)
    {
        spawnedBoard = Instantiate(chessBoardPrefab, position, rotation);
        boardSpawned = true;
        SpawnPiecesOnBoard();
    }

    void SpawnPiecesOnBoard()
    {
        if (chessPiecePrefabs == null || chessPiecePrefabs.Length == 0 || spawnedBoard == null)
            return;

        spawnedPieces.Clear();
        defaultPositions.Clear();
        FallbackSpawnGrid();
    }

    void FallbackSpawnGrid()
    {
        Vector3 boardPos = spawnedBoard.transform.position;
        float spacing = 1.2f;

        for (int i = 0; i < chessPiecePrefabs.Length; i++)
        {
            if (chessPiecePrefabs[i] == null) continue;
            Vector3 pos = boardPos + new Vector3((i - chessPiecePrefabs.Length / 2f) * spacing, 0.01f, 0f);
            GameObject piece = Instantiate(chessPiecePrefabs[i], pos, Quaternion.identity);
            piece.transform.SetParent(spawnedBoard.transform);
            spawnedPieces.Add(piece);
            defaultPositions.Add(pos);
        }
    }

    public void ResetPieces()
    {
        for (int i = 0; i < spawnedPieces.Count; i++)
        {
            if (spawnedPieces[i] != null)
                spawnedPieces[i].transform.position = defaultPositions[i];
        }
    }
}