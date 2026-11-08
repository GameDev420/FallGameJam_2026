using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public struct SaveData
    {
        public uint ThymeCollected;

        public List<UpgradeData> UnlockedUpgrades;

        // Kinda just don't need a constructor to begin with soooooooooooooo
        /*public Data(uint _ThymeCollected)
        {
            ThymeCollected = _ThymeCollected;
        }*/
    }

    public static SaveSystem Instance;

    [Header("Data")]
    [SerializeField] SaveData data;
    public SaveData Data => data;

    [Header("Events")]
    [SerializeField] UnityEvent<SaveData> onDataChanged;
    public UnityEvent<SaveData> OnDataChanged => onDataChanged;

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

    public static bool TryGetSaveData(out SaveData data)
    {
        data = default;

        if (Instance == null) return false;
        
        data = Instance.data;
        return true;
    }
}
