using UnityEngine;

public class GridSystemBehaviour : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize = new();
    [SerializeField] Vector2 tileSize = Vector2.one;
    [SerializeField] TileBehaviour tilePrefab;
    GridSystem grid;
    TileBehaviour[][] tileComponents;

    private void Awake()
    {
        grid = new(gridSize.x, gridSize.y);
        tileComponents = new TileBehaviour[gridSize.x][];

        for (int x = 0; x < grid.Grid.Length; x++)
        {
            tileComponents[x] = new TileBehaviour[gridSize.y];

            var column = grid.Grid[x];
            for (int y = 0; y < column.Length; y++)
            {
                var spawnedTile = Instantiate(tilePrefab);
                spawnedTile.Init(column[y]);
                spawnedTile.transform.SetPositionAndRotation(new(tileSize.x * x + tileSize.x, tileSize.y * y + tileSize.y), Quaternion.identity);
                tileComponents[x][y] = spawnedTile;
            }
        }
    }


}
