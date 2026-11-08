using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [Header("General")]
    public string DisplayName;
    public string UpgradeId;
    public Sprite Icon;
    public int Price;
    public int RequiredThyme = 0;
    public UpgradeData[] Prerequisites;

    [Header("Repeat")]
    public bool Repeatable = false;
    [Min(0.01f)] public float RepeatPriceMultiplier = 1.5f;
    public float RepeatThymeMultiplier = 1.2f;

    [Header("Gameplay Effects")] // yikes. only doing this for the sake of this jam I PROMISE.
    public float TimerAdditive = 0.0f;
    public float TimerMultiplier = 1.0f;
    [Space]
    public float RangeAdditive = 0.0f;
    public float RangeMultiplier = 1.0f;
    [Space]
    public float SpeedAdditive = 0.0f;
    public float SpeedMultiplier = 1.0f;
    [Space]
    public int NumFieldUpgrades = 0;
    [Space]
    public int CatAdditive = 10;
    public int ThymeAdditive = 10;

    public bool IsUnlocked()
    {
        if (SaveSystem.TryGetSaveData(out var saveData) && saveData.UnlockedUpgrades != null && saveData.UnlockedUpgrades.Count > 0)
        {
            for (int i = 0; i < saveData.UnlockedUpgrades.Count; i++)
            {
                if (saveData.UnlockedUpgrades[i] == UpgradeId) return true;
            }
        }
        return false;
    }
}
