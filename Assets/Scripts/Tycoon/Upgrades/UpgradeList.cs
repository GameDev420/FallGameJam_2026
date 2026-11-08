using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeList", menuName = "Scriptable Objects/UpgradeList")]
public class UpgradeList : ScriptableObject
{
    [Tooltip("Consists of all possible upgrades that can be used by objects who reference this. Idea is to have one large list for all game upgrades, since this is a relatively small project.")]
    public UpgradeData[] AllUpgrades;

    public UpgradeData FromId(string id)
    {
        for (int i = 0; i < AllUpgrades.Length; i++)
        {
            if (AllUpgrades[i].UpgradeId.Equals(id)) return AllUpgrades[i];
        }

        return null;
    }

    public List<UpgradeData> FromId(List<string> ids)
    {
        var upgrades = new List<UpgradeData>();
        for (int i = 0; i < ids.Count; i++)
        {
            var upgrade = FromId(ids[i]);
            if (upgrade != null) upgrades.Add(upgrade);
        }
        return upgrades;
    }

    public List<string> GetAllUpgradeIds()
    {
        var ids = new List<string>();
        for (int i = 0; i < AllUpgrades.Length; i++)
        {
            ids.Add(AllUpgrades[i].UpgradeId);
        }
        return ids;
    }
}
