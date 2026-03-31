using UnityEngine;

public class ChessSetup_Diya : MonoBehaviour
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

    void Start()
    {
        SpawnPieces();
    }

    void OnEnable()
    {
        if (whiteParent != null || blackParent != null)
            SpawnPieces();
    }

    void SpawnPieces()
    {
        if (whiteParent != null) Destroy(whiteParent);
        if (blackParent != null) Destroy(blackParent);

        if (ChessModel == null) { Debug.LogError("ChessSetup_Diya: No ChessModel assigned!"); return; }
        if (chessBoardModel == null) { Debug.LogError("ChessSetup_Diya: No chessBoardModel assigned!"); return; }

        whiteParent = new GameObject("WhitePieces");
        whiteParent.transform.SetParent(ChessModel.transform, false);

        blackParent = new GameObject("BlackPieces");
        blackParent.transform.SetParent(ChessModel.transform, false);

        CreateBoardPositions();
        PlacePieces();
    }

    void CreateBoardPositions()
    {
        Renderer boardRenderer = chessBoardModel.GetComponentInChildren<Renderer>();
        if (boardRenderer == null)
        {
            Debug.LogError("ChessSetup_Diya: No Renderer found on chessBoardModel or its children!");
            return;
        }

        // Use local bounds scaled by lossyScale to correctly handle the 0.02 parent scale
        Bounds localBounds = boardRenderer.localBounds;
        Vector3 lossyScale = chessBoardModel.transform.lossyScale;

        float boardSizeX = localBounds.size.x * lossyScale.x;
        float boardSizeZ = localBounds.size.z * lossyScale.z;

        float squareSizeX = (boardSizeX - boardInset * 2f) / 8f;
        float squareSizeZ = (boardSizeZ - boardInset * 2f) / 8f;
        squareSize = Mathf.Min(squareSizeX, squareSizeZ);

        Vector3 center = boardRenderer.bounds.center;
        Vector3 right = chessBoardModel.transform.right;
        Vector3 forward = chessBoardModel.transform.forward;

        Vector3 bottomLeft =
            center
            - right * (boardSizeX / 2f - boardInset)
            - forward * (boardSizeZ / 2f - boardInset);

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
        if (mat == null) return;
        Renderer[] renderers = piece.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
            r.material = mat;
    }

    void PlacePieces()
    {
        Quaternion whiteRotation = chessBoardModel.transform.rotation;
        Quaternion blackRotation = chessBoardModel.transform.rotation * Quaternion.Euler(0, 180, 0);

        for (int i = 0; i < 8; i++)
        {
            GameObject wPawn = Instantiate(pawnPrefab, boardPositions[i, 1], whiteRotation, whiteParent.transform);
            wPawn.transform.localScale = pawnScale;
            ApplyMaterial(wPawn, whitePiece);

            GameObject bPawn = Instantiate(pawnPrefab, boardPositions[i, 6], blackRotation, blackParent.transform);
            bPawn.transform.localScale = pawnScale;
            ApplyMaterial(bPawn, blackPiece);
        }

        PlacePiece(rookPrefab,   boardPositions[0, 0], whiteParent.transform, rookScale,   whitePiece, whiteRotation);
        PlacePiece(rookPrefab,   boardPositions[7, 0], whiteParent.transform, rookScale,   whitePiece, whiteRotation);
        PlacePiece(rookPrefab,   boardPositions[0, 7], blackParent.transform, rookScale,   blackPiece, blackRotation);
        PlacePiece(rookPrefab,   boardPositions[7, 7], blackParent.transform, rookScale,   blackPiece, blackRotation);

        PlacePiece(knightPrefab, boardPositions[1, 0], whiteParent.transform, knightScale, whitePiece, whiteRotation);
        PlacePiece(knightPrefab, boardPositions[6, 0], whiteParent.transform, knightScale, whitePiece, whiteRotation);
        PlacePiece(knightPrefab, boardPositions[1, 7], blackParent.transform, knightScale, blackPiece, blackRotation);
        PlacePiece(knightPrefab, boardPositions[6, 7], blackParent.transform, knightScale, blackPiece, blackRotation);

        PlacePiece(bishopPrefab, boardPositions[2, 0], whiteParent.transform, bishopScale, whitePiece, whiteRotation);
        PlacePiece(bishopPrefab, boardPositions[5, 0], whiteParent.transform, bishopScale, whitePiece, whiteRotation);
        PlacePiece(bishopPrefab, boardPositions[2, 7], blackParent.transform, bishopScale, blackPiece, blackRotation);
        PlacePiece(bishopPrefab, boardPositions[5, 7], blackParent.transform, bishopScale, blackPiece, blackRotation);

        PlacePiece(queenPrefab,  boardPositions[3, 0], whiteParent.transform, queenScale,  whitePiece, whiteRotation);
        PlacePiece(queenPrefab,  boardPositions[3, 7], blackParent.transform, queenScale,  blackPiece, blackRotation);

        PlacePiece(kingPrefab,   boardPositions[4, 0], whiteParent.transform, kingScale,   whitePiece, whiteRotation);
        PlacePiece(kingPrefab,   boardPositions[4, 7], blackParent.transform, kingScale,   blackPiece, blackRotation);
    }

    void PlacePiece(GameObject prefab, Vector3 position, Transform parent, Vector3 scale, Material mat, Quaternion rotation)
    {
        GameObject piece = Instantiate(prefab, position, rotation, parent);
        piece.transform.localScale = scale;
        ApplyMaterial(piece, mat);
    }
}