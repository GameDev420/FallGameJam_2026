using System.Collections.Generic;
using UnityEngine;
using static GridSystem;

public class GridSystemBehaviour : MonoBehaviour
{
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
        gridSize = new Vector2Int(Mathf.Abs(gridSize.x), Mathf.Abs(gridSize.y));

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

        if (gridParent != null) Destroy(gridParent.gameObject);
        
        gridParent = new GameObject("Grid Parent").transform;
        gridParent.SetParent(transform);

        var additivePos = centered ? tileSize * gridSize * -0.5f + tileSize * 0.5f : tileSize * 0.5f;
        var multiplicativePos = tileSize;

        for (int x = 0; x < grid.Grid.Length; x++)
        {
            tileComponents[x] = new TileBehaviour[gridSize.y];

            var columnParent = new GameObject($"Col.{x} Parent").transform;
            columnParent.SetParent(gridParent);

            var column = grid.Grid[x];
            for (int y = 0; y < column.Length; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, columnParent);
                spawnedTile.Init(new(column[y], tileSize));
                spawnedTile.transform.SetLocalPositionAndRotation(new(x * multiplicativePos.x + additivePos.x, y * multiplicativePos.y + additivePos.y, spawnedTile.transform.localPosition.z), Quaternion.identity);
                tileComponents[x][y] = spawnedTile;
            }
        }
    }
}
