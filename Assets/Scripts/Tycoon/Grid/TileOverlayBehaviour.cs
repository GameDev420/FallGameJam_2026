using UnityEngine;
using static GridSystem;

public class TileOverlayBehaviour : MonoBehaviour
{
    public struct InitData
    {
        public TileOverlay Data;
        public Vector2 Scale;

        public InitData(TileOverlay _Data, Vector2 _Scale)
        {
            Data = _Data;
            Scale = _Scale;
        }
    }

    [SerializeField] SpriteRenderer m_Renderer;
    protected TileOverlay data;

    public void Init(InitData initData, bool Force = false)
    {
        if (!Force && initData.Data == data) return;

        data = initData.Data;

        if (m_Renderer == null) m_Renderer = GetComponentInChildren<SpriteRenderer>();
        InitSpriteRenderer(m_Renderer, initData);
    }
}
