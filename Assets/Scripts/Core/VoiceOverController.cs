using UnityEngine;

public class VoiceOverController : MonoBehaviour
{
    [SerializeField] PlayableSFXObject explosionSFX;
    [SerializeField] PlayableSFXObject voiceSFX;

    bool explosionTriggered = false;
    AudioSource source;

    private void Start()
    {
        voiceSFX.SFX.TryPlay(this, out source);
    }

    private void Update()
    {
        if (explosionTriggered || source == null) return;

        if (!source.isPlaying)
        {
            explosionSFX.SFX.TryPlay(this);
            explosionTriggered = true;
        }
    }
}
