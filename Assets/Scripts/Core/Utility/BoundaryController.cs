using UnityEngine;

[DefaultExecutionOrder(-50)]
public class BoundaryController : MonoBehaviour
{
    [SerializeField] EdgeCollider2D edgeBoundaryPrefab;
    Camera cam;

    EdgeCollider2D rightBoundary, leftBoundary, topBoundary, bottomBoundary;
    public EdgeCollider2D RightBoundary => rightBoundary;
    public EdgeCollider2D LeftBoundary => leftBoundary;
    public EdgeCollider2D TopBoundary => topBoundary;
    public EdgeCollider2D BottomBoundary => bottomBoundary;

    private void Awake()
    {
        cam = Camera.main;

        var bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0));
        var bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0));
        var topRight = cam.ViewportToWorldPoint(new Vector3(1, 1));
        var topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1));

        leftBoundary = Instantiate(edgeBoundaryPrefab, transform, true);
        leftBoundary.SetPoints(new() { bottomLeft, bottomRight });

        rightBoundary = Instantiate(edgeBoundaryPrefab, transform, true);
        rightBoundary.SetPoints(new() { bottomRight, topRight });

        topBoundary = Instantiate(edgeBoundaryPrefab, transform, true);
        topBoundary.SetPoints(new() { topRight, topLeft });

        bottomBoundary = Instantiate(edgeBoundaryPrefab, transform, true);
        bottomBoundary.SetPoints(new() { topLeft, bottomLeft });
    }
}
