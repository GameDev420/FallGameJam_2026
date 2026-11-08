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
        public struct TileInitData
        {
            public int OrderInLayer;
            public Sprite BaseSprite;
            public Color BaseColor;
            public List<TileOverlay> Overlays;
            public OverlayEventHandler OnOverlaysUpdated;

            public TileInitData(int _OrderInLayer = -1, Color _BaseColor = default, Sprite _BaseSprite = null, List<TileOverlay> _Overlays = null, OverlayEventHandler _OnOverlaysUpdated = null)
            {
                OrderInLayer = _OrderInLayer;
                BaseColor = _BaseColor;
                BaseSprite = _BaseSprite;
                Overlays = _Overlays;
                OnOverlaysUpdated = _OnOverlaysUpdated;
            }
        }

        public Tile(TileInitData initData)
        {
            BaseOrderInLayer = initData.OrderInLayer;
            BaseColor = initData.BaseColor;
            BaseSprite = initData.BaseSprite;
            Overlays = initData.Overlays;
            OnOverlaysUpdated = initData.OnOverlaysUpdated;
        }

        public delegate void OverlayEventHandler();
        public event OverlayEventHandler OnOverlaysUpdated;

        public int BaseOrderInLayer;
        public Color BaseColor;
        public Sprite BaseSprite;
        public List<TileOverlay> Overlays;

        public readonly void AddOverlay(TileOverlay overlay)
        {
            Overlays.Add(overlay);
            if (OnOverlaysUpdated != null) OnOverlaysUpdated.Invoke();
        }
        public readonly void RemoveOverlay(TileOverlay overlay)
        {
            if (Overlays.Contains(overlay)) Overlays.Remove(overlay);
            if (OnOverlaysUpdated != null) OnOverlaysUpdated.Invoke();
        }
        public readonly void RemoveOverlay(int overlayIndex)
        {
            if (overlayIndex >= Overlays.Count) return;
            Overlays.RemoveAt(overlayIndex);
            if (OnOverlaysUpdated != null) OnOverlaysUpdated.Invoke();
        }
    }

    // For all objects rendered *on top* of the base tile, such as thyme or grass.
    [Serializable]
    public class TileOverlay : IComparable<TileOverlay>
    {
        public TileOverlay() { }
        public TileOverlay(int _OrderInLayer, Color _OverlayColor, Sprite _OverlaySprite)
        {
            OrderInLayer = _OrderInLayer;
            OverlayColor = _OverlayColor;
            OverlaySprite = _OverlaySprite;
        }


        [Header("Optional")]
        [Tooltip("Higher = Appears First")]
        public int OrderInLayer;
        public Color OverlayColor;
        public Sprite OverlaySprite;

        public virtual int CompareTo(TileOverlay other)
        {
            if (other == null) return 1;

            return OrderInLayer.CompareTo(other.OrderInLayer);
        }
    }

    #endregion

    #region Variables

    [SerializeField] Vector2 gridSize;

    readonly Tile[][] grid;

    public Tile[][] Grid => grid;

    #endregion

    #region Constructors

    public GridSystem()
    {
        grid = UniformGrid(0, 0, default);
    }
    public GridSystem(int x, int y, Tile.TileInitData initData = default)
    {
        grid = UniformGrid(x, y, initData);
    }
    public GridSystem(Tile[][] Grid)
    {
        grid = Grid;
    }

    #endregion

    #region Methods

    public bool TryGetTile(int x, int y, out Tile tile)
    {
        tile = default;
        if (x >= grid.Length || grid[x].Length >= y) return false;

        tile = grid[x][y];
        return true;
    }

    static Tile[][] UniformGrid(int width, int height, Tile.TileInitData initData)
    {
        var grid = new Tile[width][];
        
        for (int x = 0; x < width; x++)
        {
            var column = new Tile[height];

            if (height > 0)
            {
                for (int y = 0; y < height; y++)
                {
                    // Alledgedly "shorthand" btw
                    column[y] = new(initData);
                }
            }

            grid[x] = column;
        }

        return grid;
    }

    public static void InitSpriteRenderer(SpriteRenderer renderer, TileBehaviour.TileBehaviourInitData tile)
    {
        if (renderer == null) return;

        renderer.sortingOrder = tile.Data.BaseOrderInLayer;
        renderer.color = tile.Data.BaseColor;
        if (tile.Data.BaseSprite != null) renderer.sprite = tile.Data.BaseSprite;

        renderer.size = tile.Scale;
    }
    public static void InitSpriteRenderer(SpriteRenderer renderer, TileOverlayBehaviour.InitData overlay)
    {
        if (renderer == null) return;

        renderer.sortingOrder = overlay.Data.OrderInLayer;
        renderer.color = overlay.Data.OverlayColor;
        if (overlay.Data.OverlaySprite != null) renderer.sprite = overlay.Data.OverlaySprite;

        renderer.size = overlay.Scale;
    }

    #endregion
}
