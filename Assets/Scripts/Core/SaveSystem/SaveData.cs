using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Data
    {
        public uint ThymeCollected;

        // Kinda just don't need a constructor to begin with soooooooooooooo
        /*public Data(uint _ThymeCollected)
        {
            ThymeCollected = _ThymeCollected;
        }*/
    }

    public static SaveSystem Instance;

    [Header("Data")]
    [SerializeField] Data data;

    [Header("Events")]
    [SerializeField] UnityEvent<Data> onDataChanged;
    public UnityEvent<Data> OnDataChanged => onDataChanged;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else
        {
            Instance = this;
            if (transform.parent != null) transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}
