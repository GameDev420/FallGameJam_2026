using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    public struct SaveData
    {
        public uint ThymeCollected;
        public uint NumFieldUpgrades;

        public List<UpgradeData> UnlockedUpgrades;
        public Dictionary<UpgradeData, int> UpgradeRepeatCounts;
        public float TimerMultiplier;
        public float RangeMultiplier;
        public float SpeedMultiplier;
        
        public float TimerAdditive;
        public float RangeAdditive;
        public float SpeedAdditive;

        public int CatAdditive;
        public int ThymeAdditive;

        public int GetUpgradeRepeatCount(UpgradeData upgrade)
        {
            if (UpgradeRepeatCounts == null) return 0;
            if (!UpgradeRepeatCounts.TryGetValue(upgrade, out var count)) return 0;
            return count;
        }

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

            data = new()
            {
                ThymeCollected = 0,
                UnlockedUpgrades = new List<UpgradeData>(),
                UpgradeRepeatCounts = new Dictionary<UpgradeData, int>(),
                TimerMultiplier = 1.0f,
                RangeMultiplier = 1.0f,
                SpeedMultiplier = 1.0f,
            };
        }
    }

    public static bool TryGetSaveData(out SaveData data)
    {
        data = default;

        if (Instance == null) return false;
        
        data = Instance.data;
        return true;
    }

    public static void UpdateSaveData(SaveData newData)
    {
        if (Instance == null) return;
        Instance.data = newData;
        if (Instance.onDataChanged != null) Instance.onDataChanged.Invoke(newData);
    }
}
