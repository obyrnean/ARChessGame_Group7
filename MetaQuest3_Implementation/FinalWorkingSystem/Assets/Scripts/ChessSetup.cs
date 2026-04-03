using UnityEngine;
using System.Collections.Generic;

public class ChessSetup : MonoBehaviour
{
    [Header("ChessModel Parent")]
    public GameObject ChessModel;

    [Header("Board Model")]
    public GameObject chessBoardModel;

    [Header("Board Tuning")]
    public float boardInset = 0.0f;
    public float yOffset = 0.05f;

    [Header("Prefabs")]
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;
    public GameObject bishopPrefab;
    public GameObject queenPrefab;
    public GameObject kingPrefab;

    [Header("Extra")]
    public GameObject extraBlockPrefab;

    [Header("Materials")]
    public Material whitePiece;
    public Material blackPiece;

    [Header("Piece Sizes")]
    public Vector3 pawnScale = Vector3.one;
    public Vector3 rookScale = Vector3.one;
    public Vector3 knightScale = Vector3.one;
    public Vector3 bishopScale = Vector3.one;
    public Vector3 queenScale = Vector3.one;
    public Vector3 kingScale = Vector3.one;

    private Vector3[,] boardPositions = new Vector3[8, 8];
    private float squareSize;

    private GameObject whiteParent;
    private GameObject blackParent;


    private List<GameObject> allPieces = new List<GameObject>();
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> initialRotations = new Dictionary<GameObject, Quaternion>();

    void Start()
    {
        if (ChessModel == null || chessBoardModel == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        whiteParent = new GameObject("WhitePieces");
        whiteParent.transform.SetParent(ChessModel.transform, false);

        blackParent = new GameObject("BlackPieces");
        blackParent.transform.SetParent(ChessModel.transform, false);

        CreateBoardPositions();
        PlacePieces();
        SpawnExtraBlocks();
    }

    void CreateBoardPositions()
    {
        Renderer boardRenderer = chessBoardModel.GetComponent<Renderer>();
        Vector3 boardSize = boardRenderer.bounds.size;
        Vector3 center = chessBoardModel.transform.position;

        float squareSizeX = (boardSize.x - boardInset * 2f) / 8f;
        float squareSizeZ = (boardSize.z - boardInset * 2f) / 8f;
        squareSize = Mathf.Min(squareSizeX, squareSizeZ);

        Vector3 right = chessBoardModel.transform.right;
        Vector3 forward = chessBoardModel.transform.forward;

        Vector3 bottomLeft =
            center
            - right * (boardSize.x / 2f - boardInset)
            - forward * (boardSize.z / 2f - boardInset);

        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                Vector3 pos =
                    bottomLeft
                    + right * ((x + 0.5f) * squareSize)
                    + forward * ((z + 0.5f) * squareSize);

                pos.y = boardRenderer.bounds.max.y + yOffset;
                boardPositions[x, z] = pos;
            }
        }
    }

    void ApplyMaterial(GameObject piece, Material mat)
    {
        foreach (Renderer r in piece.GetComponentsInChildren<Renderer>())
        {
            r.material = mat;
        }
    }

    void RegisterPiece(GameObject piece)
    {
        allPieces.Add(piece);
        initialPositions[piece] = piece.transform.position;
        initialRotations[piece] = piece.transform.rotation;
    }

    void PlacePieces()
    {
        Quaternion whiteRot = chessBoardModel.transform.rotation;
        Quaternion blackRot = chessBoardModel.transform.rotation * Quaternion.Euler(0, 180, 0);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            GameObject wPawn = Instantiate(pawnPrefab, boardPositions[i, 1], whiteRot, whiteParent.transform);
            wPawn.transform.localScale = pawnScale;
            ApplyMaterial(wPawn, whitePiece);
            RegisterPiece(wPawn);

            GameObject bPawn = Instantiate(pawnPrefab, boardPositions[i, 6], blackRot, blackParent.transform);
            bPawn.transform.localScale = pawnScale;
            ApplyMaterial(bPawn, blackPiece);
            RegisterPiece(bPawn);
        }

        // Other pieces
        PlacePiece(rookPrefab, boardPositions[0, 0], whiteParent.transform, rookScale, whitePiece, whiteRot);
        PlacePiece(rookPrefab, boardPositions[7, 0], whiteParent.transform, rookScale, whitePiece, whiteRot);
        PlacePiece(rookPrefab, boardPositions[0, 7], blackParent.transform, rookScale, blackPiece, blackRot);
        PlacePiece(rookPrefab, boardPositions[7, 7], blackParent.transform, rookScale, blackPiece, blackRot);

        PlacePiece(knightPrefab, boardPositions[1, 0], whiteParent.transform, knightScale, whitePiece, whiteRot);
        PlacePiece(knightPrefab, boardPositions[6, 0], whiteParent.transform, knightScale, whitePiece, whiteRot);
        PlacePiece(knightPrefab, boardPositions[1, 7], blackParent.transform, knightScale, blackPiece, blackRot);
        PlacePiece(knightPrefab, boardPositions[6, 7], blackParent.transform, knightScale, blackPiece, blackRot);

        PlacePiece(bishopPrefab, boardPositions[2, 0], whiteParent.transform, bishopScale, whitePiece, whiteRot);
        PlacePiece(bishopPrefab, boardPositions[5, 0], whiteParent.transform, bishopScale, whitePiece, whiteRot);
        PlacePiece(bishopPrefab, boardPositions[2, 7], blackParent.transform, bishopScale, blackPiece, blackRot);
        PlacePiece(bishopPrefab, boardPositions[5, 7], blackParent.transform, bishopScale, blackPiece, blackRot);

        PlacePiece(queenPrefab, boardPositions[3, 0], whiteParent.transform, queenScale, whitePiece, whiteRot);
        PlacePiece(queenPrefab, boardPositions[3, 7], blackParent.transform, queenScale, blackPiece, blackRot);

        PlacePiece(kingPrefab, boardPositions[4, 0], whiteParent.transform, kingScale, whitePiece, whiteRot);
        PlacePiece(kingPrefab, boardPositions[4, 7], blackParent.transform, kingScale, blackPiece, blackRot);
    }

    void PlacePiece(GameObject prefab, Vector3 pos, Transform parent, Vector3 scale, Material mat, Quaternion rot)
    {
        GameObject piece = Instantiate(prefab, pos, rot, parent);
        piece.transform.localScale = scale;
        ApplyMaterial(piece, mat);
        RegisterPiece(piece);
    }

    void SpawnExtraBlocks()
    {
        if (extraBlockPrefab == null) return;

        Instantiate(extraBlockPrefab, whiteParent.transform).transform.localPosition = Vector3.zero;
        Instantiate(extraBlockPrefab, blackParent.transform).transform.localPosition = Vector3.zero;
    }

    // RESET FUNCTION
    public void ResetBoard()
    {
        foreach (GameObject piece in allPieces)
        {
            if (piece != null)
            {
                piece.transform.position = initialPositions[piece];
                piece.transform.rotation = initialRotations[piece];

                Rigidbody rb = piece.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                piece.SetActive(true); // revive captured pieces if needed
            }
        }
    }
}