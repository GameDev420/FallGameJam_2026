using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [Header("General")]
    public string DisplayName;
    public Sprite Icon;
    public int Price;
    public int RequiredThyme = 0;
    public UpgradeData[] Prerequisites;

    [Header("Repeat")]
    public bool Repeatable = false;
    [Min(0.01f)] public float RepeatPriceMultiplier = 1.5f;
    public float RepeatThymeMultiplier = 1.2f;

    public bool IsUnlocked()
    {
        if (SaveSystem.TryGetSaveData(out var saveData) && saveData.UnlockedUpgrades != null && saveData.UnlockedUpgrades.Count > 0)
        {
            for (int i = 0; i < saveData.UnlockedUpgrades.Count; i++)
            {
                if (saveData.UnlockedUpgrades[i] == this) return true;
            }
        }
        return false;
    }
}
