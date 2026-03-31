using UnityEngine;

public class ChessSetup : MonoBehaviour
{
    [Header("ChessModel Parent")]
    public GameObject ChessModel; // This is the top-level parent (also child of the iamge target)

    [Header("Board Model")]
    public GameObject chessBoardModel; // The actual 3D board mesh inside ChessModel

    [Header("Board Tuning")]
    public float boardInset = 0.0f;   // adjust if your board has borders
    public float yOffset = 0.05f;     // lifts pieces above board

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
        if (ChessModel == null)
        {
            Debug.LogError("No ChessModel parent assigned!");
            return;
        }

        if (chessBoardModel == null)
        {
            Debug.LogError("No chess board model assigned!");
            return;
        }

        // Create parent objects as children of ChessModel (or whichever name you gave)
        whiteParent = new GameObject("WhitePieces");
        whiteParent.transform.SetParent(ChessModel.transform, false);

        blackParent = new GameObject("BlackPieces");
        blackParent.transform.SetParent(ChessModel.transform, false);

        CreateBoardPositions();
        PlacePieces();
    }

    void CreateBoardPositions()
    {
        Renderer boardRenderer = chessBoardModel.GetComponent<Renderer>();
        if (boardRenderer == null)
        {
            Debug.LogError("Chess board must have a Renderer component!");
            return;
        }

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
        Renderer[] renderers = piece.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material = mat;
        }
    }

    void PlacePieces()
    {
        Quaternion whiteRotation = chessBoardModel.transform.rotation;
        Quaternion blackRotation = chessBoardModel.transform.rotation * Quaternion.Euler(0, 180, 0);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            GameObject wPawn = Instantiate(pawnPrefab, boardPositions[i, 1], whiteRotation, whiteParent.transform);
            wPawn.transform.localScale = pawnScale;
            ApplyMaterial(wPawn, whitePiece);

            GameObject bPawn = Instantiate(pawnPrefab, boardPositions[i, 6], blackRotation, blackParent.transform);
            bPawn.transform.localScale = pawnScale;
            ApplyMaterial(bPawn, blackPiece);
        }

        // Rooks
        PlacePiece(rookPrefab, boardPositions[0, 0], whiteParent.transform, rookScale, whitePiece, whiteRotation);
        PlacePiece(rookPrefab, boardPositions[7, 0], whiteParent.transform, rookScale, whitePiece, whiteRotation);
        PlacePiece(rookPrefab, boardPositions[0, 7], blackParent.transform, rookScale, blackPiece, blackRotation);
        PlacePiece(rookPrefab, boardPositions[7, 7], blackParent.transform, rookScale, blackPiece, blackRotation);

        // Knights
        PlacePiece(knightPrefab, boardPositions[1, 0], whiteParent.transform, knightScale, whitePiece, whiteRotation);
        PlacePiece(knightPrefab, boardPositions[6, 0], whiteParent.transform, knightScale, whitePiece, whiteRotation);
        PlacePiece(knightPrefab, boardPositions[1, 7], blackParent.transform, knightScale, blackPiece, blackRotation);
        PlacePiece(knightPrefab, boardPositions[6, 7], blackParent.transform, knightScale, blackPiece, blackRotation);

        // Bishops
        PlacePiece(bishopPrefab, boardPositions[2, 0], whiteParent.transform, bishopScale, whitePiece, whiteRotation);
        PlacePiece(bishopPrefab, boardPositions[5, 0], whiteParent.transform, bishopScale, whitePiece, whiteRotation);
        PlacePiece(bishopPrefab, boardPositions[2, 7], blackParent.transform, bishopScale, blackPiece, blackRotation);
        PlacePiece(bishopPrefab, boardPositions[5, 7], blackParent.transform, bishopScale, blackPiece, blackRotation);

        // Queens
        PlacePiece(queenPrefab, boardPositions[3, 0], whiteParent.transform, queenScale, whitePiece, whiteRotation);
        PlacePiece(queenPrefab, boardPositions[3, 7], blackParent.transform, queenScale, blackPiece, blackRotation);

        // Kings
        PlacePiece(kingPrefab, boardPositions[4, 0], whiteParent.transform, kingScale, whitePiece, whiteRotation);
        PlacePiece(kingPrefab, boardPositions[4, 7], blackParent.transform, kingScale, blackPiece, blackRotation);
    }

    void PlacePiece(GameObject prefab, Vector3 position, Transform parent, Vector3 scale, Material mat, Quaternion rotation)
    {
        GameObject piece = Instantiate(prefab, position, rotation, parent);
        piece.transform.localScale = scale;
        ApplyMaterial(piece, mat);
    }
}
