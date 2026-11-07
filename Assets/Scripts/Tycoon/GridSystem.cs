using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    #region Data Types

    /// <summary>
    /// Basic data for every tile (typically for ground).
    /// </summary>
    [Serializable]
    public struct Tile
    {
        public Sprite BaseSprite;
        public List<TileOverlay> overlays;

        public void AddOverlay()
        {

        }
        public void RemoveOverlay()
        {

        }
    }

    // For all objects rendered *on top* of the base tile, such as thyme or grass.
    public class TileOverlay : IComparable<TileOverlay>
    {
        [Tooltip("Higher = Appears First")]
        public int Priority;

        [Tooltip("Optional")]
        public Sprite OverlaySprite;

        public virtual int CompareTo(TileOverlay other)
        {
            if (other == null) return 1;

            return Priority.CompareTo(other.Priority);
        }

    }

    #endregion

    [SerializeField] Vector2 gridSize;

    readonly Tile[][] grid;

    public Tile[][] Grid => grid;

    public GridSystem()
    {
        grid = new Tile[0][];
    }
    public GridSystem(int x, int y)
    {
        grid = new Tile[x][];
        for (int i = 0; i < x; i++)
            grid[i] = new Tile[y];
    }
    public GridSystem(Tile[][] Grid)
    {
        grid = Grid;
    }

    public bool TryGetTile(int x, int y, out Tile tile)
    {
        tile = default;
        if (x >= grid.Length || grid[x].Length >= y) return false;

        tile = grid[x][y];
        return true;
    }
}
