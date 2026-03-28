using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class CubeSpawner : MonoBehaviour
{
    public GameObject chessBoardPrefab;
    public float editorSpawnDistance = 1.5f;

    private ARRaycastManager raycastManager;
    private GameObject spawnedBoard;
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
        // ChessSetup on the chessModel prefab handles piece placement automatically
    }

    public void ResetPieces()
    {
        // Find ChessSetup on the spawned board and re-run setup
        if (spawnedBoard != null)
        {
            ChessSetup setup = spawnedBoard.GetComponentInChildren<ChessSetup>();
            if (setup != null)
            {
                // Destroy existing pieces and re-place
                Destroy(spawnedBoard);
                boardSpawned = false;
            }
        }
    }
}