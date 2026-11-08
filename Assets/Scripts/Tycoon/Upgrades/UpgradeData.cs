using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string DisplayName;
    public int Price;
    public int RequiredThyme = 0;
    public UpgradeData[] Prerequisites;

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
