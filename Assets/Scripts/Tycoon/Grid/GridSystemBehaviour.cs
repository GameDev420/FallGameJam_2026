using System.Collections.Generic;
using UnityEngine;
using static GridSystem;

public class GridSystemBehaviour : MonoBehaviour
{
    const int MaximumTiles = 30000;

    [Header("Config")]
    [SerializeField] bool centered = true;
    [SerializeField] Vector2Int gridSize = new();
    [SerializeField] Vector2 tileSize = Vector2.one;

    [Header("References")]
    [SerializeField] TileBehaviour tilePrefab;

    [Header("Default Grid Tile")]
    [SerializeField] int baseOrderInLayer = -1;
    [SerializeField] Color baseColor = Color.white;
    [SerializeField] Sprite baseSprite;
    [SerializeField] List<TileOverlay> baseOverlays;
    
    GridSystem grid;
    TileBehaviour[][] tileComponents;
    Transform gridParent;

    bool refresh;

    private void OnValidate()
    {
        gridSize = new Vector2Int(Mathf.Min(Mathf.Abs(gridSize.x), MaximumTiles / gridSize.y), Mathf.Min(Mathf.Abs(gridSize.y), MaximumTiles / gridSize.x));

        // Pet-peeve of mine
        if (baseOverlays != null && baseOverlays.Count > 0)
        {
            for (int i = 0; i < baseOverlays.Count; i++)
            {
                if (baseOverlays[i].OverlayColor == default) baseOverlays[i].OverlayColor = Color.white;
            }
        }

        if (Application.isPlaying) refresh = true;
    }

    private void Update()
    {
        if (refresh) InitGrid();
        refresh = false;
    }

    private void Awake()
    {
        InitGrid();
    }

    void InitGrid()
    {
        grid = new(gridSize.x, gridSize.y, new(baseOrderInLayer, baseColor, baseSprite, baseOverlays == null ? null : new(baseOverlays)));
        tileComponents = new TileBehaviour[gridSize.x][];

        if (gridParent != null)
        {
            Destroy(gridParent.gameObject);
            if (TileBehaviour.OverlayPool != null && TileBehaviour.OverlayPool.AllObjects.Count > 0) TileBehaviour.OverlayPool.ReturnAllObjects();
        }
        
        gridParent = new GameObject("Grid Parent").transform;
        gridParent.SetParent(transform);
        gridParent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        var additivePos = centered ? tileSize * gridSize * -0.5f + tileSize * 0.5f : tileSize * 0.5f;
        var multiplicativePos = tileSize;

        for (int x = 0; x < grid.Grid.Length; x++)
        {
            tileComponents[x] = new TileBehaviour[gridSize.y];

            var columnParent = new GameObject($"Col.{x} Parent").transform;
            columnParent.SetParent(gridParent);
            columnParent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            var column = grid.Grid[x];
            for (int y = 0; y < column.Length; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, columnParent);
                spawnedTile.transform.SetLocalPositionAndRotation(new(x * multiplicativePos.x + additivePos.x, y * multiplicativePos.y + additivePos.y, spawnedTile.transform.localPosition.z), Quaternion.identity);
                spawnedTile.Init(new(column[y], tileSize));
                tileComponents[x][y] = spawnedTile;
            }
        }
    }

    [ContextMenu("Add Overlays")]
    void AddOverlays()
    {
        if (!Application.isEditor || !Application.isPlaying) return;

        for (int x = 0; x < grid.Grid.Length; x++)
        {
            var col = grid.Grid[x];
            for (int y = 0; y < col.Length; y++)
            {
                for (int i = 0; i < baseOverlays.Count; i++)
                    col[y].AddOverlay(baseOverlays[i]);
            }
        }
    }

    public Vector2 SampleGrid(Vector2 worldPos)
    {
        var localPos = worldPos - (Vector2)transform.position;

        // Coordinates of local grid (non-centered)
        var x = Mathf.RoundToInt(Mathf.Abs(localPos.x) % (gridSize.x * tileSize.x));
        var y = Mathf.RoundToInt(Mathf.Abs(localPos.y) % (gridSize.y * tileSize.y));

        if (centered)
        {
            if (localPos.x >= 0) x += Mathf.FloorToInt(gridSize.x * 0.5f);
            if (localPos.y >= 0) y += Mathf.FloorToInt(gridSize.y * 0.5f);
        }

        return new(x, y);
    }
}
