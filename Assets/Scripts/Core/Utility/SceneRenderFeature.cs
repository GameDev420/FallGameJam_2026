using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneRenderFeature : MonoBehaviour
{
    [SerializeField] bool enable = true;
    [SerializeField] string featureName = "FullScreenPassRendererFeature";
    [SerializeField] Renderer2DData rendererData;

    private void OnEnable() => SetEffectActive(enable);
    private void OnDisable() => SetEffectActive(!enable);

    void SetEffectActive(bool active)
    {
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature != null && featureName.Equals(featureName))
            {
                if (feature.isActive != active) feature.SetActive(active);
                return;
            }
        }
    }
}
