using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FOVLight : MonoBehaviour
{
    [Header("Angle Config")]
    [Tooltip("Relative to the FOV angle.")]
    [SerializeField] [Range(0.0f, 1.0f)] float innerAngleMultiplier = 0.0f;
    [Tooltip("Relative to the FOV angle.")]
    [SerializeField] [Range(0.0f, 2.0f)] float outerAngleMultiplier = 1.1f;

    [Header("Radius Config")]
    [Tooltip("Relative to the FOV radius.")]
    [SerializeField] [Min(0.0f)] float radiusToIntensityMultiplier = 0.9f;
    [Tooltip("Relative to the FOV radius.")]
    [SerializeField] [Min(0.0f)] float innerRadiusMultiplier = 0.5f;
    [SerializeField] [Min(0.0f)] float outerRadiusMultiplier = 1.2f;

    [Header("References")]
    [SerializeField] FieldOfView fov;
    [SerializeField] Light2D[] lights;

    [Header("Debug")]
    [SerializeField] bool updateInEditor = true;

    private void Awake()
    {
        if (fov == null) fov = GetComponentInChildren<FieldOfView>();
    }

    private void Start() => UpdateLightAngle();

    private void OnValidate()
    {
        if (updateInEditor && Application.isPlaying) UpdateLightAngle();
    }

    void UpdateLightAngle()
    {
        if (fov == null || lights == null || lights.Length == 0) return;

        var outerAngle = fov.angle * outerAngleMultiplier;
        var innerAngle = Mathf.Min(fov.angle * innerAngleMultiplier, outerAngle);
        var intensity = fov.radius * radiusToIntensityMultiplier;
        var outerRadius = fov.radius * outerRadiusMultiplier;
        var innerRadius = Mathf.Min(fov.radius * innerRadiusMultiplier, outerRadius);

        for (int i = 0; i < lights.Length; i++)
        {
            var l = lights[i];
            l.pointLightInnerAngle = innerAngle;
            l.pointLightOuterAngle = outerAngle;
            l.intensity = intensity;
            l.pointLightInnerRadius = innerRadius;
            l.pointLightOuterRadius = outerRadius;
        }
    }
}
