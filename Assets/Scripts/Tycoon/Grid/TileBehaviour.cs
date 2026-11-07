using UnityEngine;
using static GridSystem;

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

    const int InitOverlayPoolSize = 50;
    static ObjectPool<TileOverlayBehaviour> overlayPool;

    protected Tile data;

    [Header("Config")]
    [Header("References")]
    [SerializeField] SpriteRenderer m_Renderer;
    [SerializeField] TileOverlayBehaviour overlayPrefab;
    Transform overlayParent;
    bool subscribed = false; // handles subscription to OnOverlaysUpdated callback to avoid memory leaks

    public void Init(TileBehaviourInitData initData)
    {
        if (subscribed) data.OnOverlaysUpdated -= OnOverlaysUpdated;

        data = initData.Data;

        if (m_Renderer == null) m_Renderer = GetComponentInChildren<SpriteRenderer>();
        InitSpriteRenderer(m_Renderer, initData);

        // Setup overlay parent
        if (overlayParent != null) Destroy(overlayParent.gameObject);
        overlayParent = new GameObject("Tile Overlay Parent").transform;
        overlayParent.SetParent(transform);

        // Create overlay pool (if needed)
        if (overlayPool == null) overlayPool = new(overlayPrefab, InitOverlayPoolSize);

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
        int overlayIndex = 0;

        var spawnedOverlays = overlayParent.GetComponentsInChildren<TileOverlayBehaviour>();

        // Handle all currently active objects
        for (int i = 0; i < spawnedOverlays.Length; i++)
        {
            var obj = spawnedOverlays[i];

            // Either re-initialize the object if there's an overlay for it...
            if (overlayIndex < data.Overlays.Count) obj.Init(new(data.Overlays[overlayIndex], m_Renderer.size));
            // Or return it to the pool if it's not being used.
            else overlayPool.ReturnObject(obj);

            overlayIndex++;
        }

        // Handle any additional overlays that need to be created
        while (overlayIndex < data.Overlays.Count)
        {
            overlayPool.GetObject().Init(new(data.Overlays[overlayIndex], m_Renderer.size));
            overlayIndex++;
        }
    }
}
