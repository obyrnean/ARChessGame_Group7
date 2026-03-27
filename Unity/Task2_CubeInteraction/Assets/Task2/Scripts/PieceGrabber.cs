using UnityEngine;
using UnityEngine.InputSystem;

public class PieceGrabber : MonoBehaviour
{
    public float grabDistance = 10f;

    private GameObject grabbedPiece = null;
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleEditorInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleEditorInput()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = arCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // Right-click to grab
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out hit, grabDistance))
                TryGrab(hit.collider.gameObject);
        }

        // Release on right button up
        if (Mouse.current.rightButton.wasReleasedThisFrame)
            grabbedPiece = null;

        // Drag grabbed piece
        if (grabbedPiece != null)
        {
            float depth = Vector3.Distance(arCamera.transform.position, grabbedPiece.transform.position);
            Vector3 worldPoint = arCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
            grabbedPiece.transform.position = worldPoint;
        }
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2) 
        {
            grabbedPiece = null;
            return;
        }

        var touch0 = Touchscreen.current.touches[0];
        var touch1 = Touchscreen.current.touches[1];

        Vector2 pos0 = touch0.position.ReadValue();
        Vector2 pos1 = touch1.position.ReadValue();
        Vector2 midpoint = (pos0 + pos1) / 2f;

        float pinchDistance = Vector2.Distance(pos0, pos1);
        bool isPinching = pinchDistance < 100f;

        Ray ray = arCamera.ScreenPointToRay(midpoint);
        RaycastHit hit;

        if (isPinching && grabbedPiece == null)
        {
            if (Physics.Raycast(ray, out hit, grabDistance))
                TryGrab(hit.collider.gameObject);
        }

        if (!isPinching)
            grabbedPiece = null;

        if (grabbedPiece != null)
        {
            float depth = Vector3.Distance(arCamera.transform.position, grabbedPiece.transform.position);
            Vector3 worldPoint = arCamera.ScreenToWorldPoint(new Vector3(midpoint.x, midpoint.y, depth));
            grabbedPiece.transform.position = worldPoint;
        }
    }

    bool IsChessPiece(GameObject obj)
    {
        return obj.name.Contains("King")   ||
               obj.name.Contains("Queen")  ||
               obj.name.Contains("Rook")   ||
               obj.name.Contains("Bishop") ||
               obj.name.Contains("Knight") ||
               obj.name.Contains("Pawn");
    }

    void TryGrab(GameObject target)
    {
        if (IsChessPiece(target))
            grabbedPiece = target;
    }
}