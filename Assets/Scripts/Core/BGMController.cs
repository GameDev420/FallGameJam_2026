using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoBehaviour
{
    static BGMController instance;

    [SerializeField] AudioClip[] bgmClips;
    int currentTrackIndex;
    AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            currentTrackIndex = -1;
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;

            instance = this;
            if (transform.parent != null) transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            SwitchTrack();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneLoader.Instance.OnSceneLoaded += OnSceneChanged;
    }
    void OnSceneChanged() => SwitchTrack();

    void SwitchTrack()
    {
        if (audioSource.isPlaying) audioSource.Stop();

        var randomIndex = currentTrackIndex;
        while (randomIndex == currentTrackIndex)
            randomIndex = Random.Range(0, bgmClips.Length);
        
        audioSource.clip = bgmClips[randomIndex];
        audioSource.Play();
        currentTrackIndex = randomIndex;
    }
}
