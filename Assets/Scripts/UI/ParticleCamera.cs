using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

// Strictly for particles that needs to render above UI elements; normal particles should not use this.
public class ParticleCamera : MonoBehaviour
{
    static ParticleCamera Instance;
    
    [SerializeField] Camera particleCam;
    public Camera ParticleCam => particleCam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Spawns particles at mouse position.
    /// Should ONLY be used for particles that need to render ABOVE UI elements.
    /// </summary>
    public static void SpawnParticlesCursor(ParticleSystem prefab) => SpawnParticlesWorld(prefab, Instance.particleCam.ScreenToWorldPoint(UnityEngine.InputSystem.Pointer.current.position.ReadValue()));
    public static void SpawnParticlesViewport(ParticleSystem prefab, Vector2 viewportPosition) => SpawnParticlesWorld(prefab, Instance.particleCam.ViewportToWorldPoint(viewportPosition));
    public static void SpawnParticlesLocal(ParticleSystem prefab, Vector3 localPosition) => SpawnParticlesWorld(prefab, Instance.particleCam.transform.TransformPoint(localPosition));

    public static void SpawnParticlesWorld(ParticleSystem prefab, Vector3 worldPosition)
    {
        var particles = Instantiate(prefab, worldPosition, prefab.transform.rotation);
        Instance.StartCoroutine(WaitToDestroyParticles(particles));
    }

    static IEnumerator WaitToDestroyParticles(ParticleSystem particles)
    {
        yield return new WaitUntil(() => !particles.isPlaying);
        Destroy(particles);
    }
}
