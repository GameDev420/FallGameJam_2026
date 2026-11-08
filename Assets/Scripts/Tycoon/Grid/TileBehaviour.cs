using UnityEngine;
using static GridSystem;
using System.Collections.Generic;

public class TileBehaviour : MonoBehaviour
{
    public struct TileBehaviourInitData
    {
        public TileBehaviourInitData(Tile _Data, Vector2 _Scale)
        {
            Data = _Data;
            Scale = _Scale;
        }

        public Tile Data;
        public Vector2 Scale;
    }
    
    public static ObjectPool<TileOverlayBehaviour> OverlayPool;

    protected Tile data;

    //[Header("Config")]
    [Header("References")]
    [SerializeField] SpriteRenderer m_Renderer;
    [SerializeField] TileOverlayBehaviour overlayPrefab;
    bool subscribed = false; // handles subscription to OnOverlaysUpdated callback to avoid memory leaks
    readonly List<TileOverlayBehaviour> localOverlays = new();
    Transform overlayParent;

    public void Init(TileBehaviourInitData initData)
    {
        if (subscribed) data.OnOverlaysUpdated -= OnOverlaysUpdated;

        data = initData.Data;

        if (m_Renderer == null) m_Renderer = GetComponentInChildren<SpriteRenderer>();
        InitSpriteRenderer(m_Renderer, initData);

        // Create overlay pool (if needed)
        if (OverlayPool == null) OverlayPool = new(overlayPrefab, 0);
        if (overlayParent == null)
        {
            overlayParent = new GameObject("Overlay Parent").transform;
            overlayParent.SetParent(transform);
        }

        OnOverlaysUpdated();

        data.OnOverlaysUpdated += OnOverlaysUpdated;
        subscribed = true;
    }

    private void OnEnable()
    {
        if (!subscribed)
        {
            data.OnOverlaysUpdated += OnOverlaysUpdated;
            subscribed = true;
        }
    }
    private void OnDisable()
    {
        if (subscribed)
        {
            data.OnOverlaysUpdated -= OnOverlaysUpdated;
            subscribed = false;
        }
    }

    // Also called when initializing a new Tile data.
    void OnOverlaysUpdated()
    {
        var overlayIndex = 0;
        
        // Handle all currently active objects
        for (int i = 0; i < localOverlays.Count; i++)
        {
            var obj = localOverlays[i];

            // Either re-initialize the object if there's an overlay for it...
            if (overlayIndex < data.Overlays.Count)
            {
                InitalizeOverlay(obj, data.Overlays[overlayIndex]);
                obj.Init(new(data.Overlays[overlayIndex], m_Renderer.size));
            }
            // Or return it to the pool if it's not being used.
            else
            {
                localOverlays.RemoveAt(i);
                OverlayPool.ReturnObject(obj);
            }

            overlayIndex++;
        }

        // Handle any additional overlays that need to be created
        while (overlayIndex < data.Overlays.Count)
        {
            InitalizeOverlay(OverlayPool.GetObject(), data.Overlays[overlayIndex]);
            overlayIndex++;
        }
    }

    void InitalizeOverlay(TileOverlayBehaviour obj, TileOverlay Data)
    {
        obj.transform.SetParent(overlayParent);
        obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        obj.Init(new(Data, m_Renderer.size));
    }
}
