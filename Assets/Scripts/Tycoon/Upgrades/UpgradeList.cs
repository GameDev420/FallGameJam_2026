using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeList", menuName = "Scriptable Objects/UpgradeList")]
public class UpgradeList : ScriptableObject
{
    [Tooltip("Consists of all possible upgrades that can be used by objects who reference this. Idea is to have one large list for all game upgrades, since this is a relatively small project.")]
    public UpgradeData[] AllUpgrades;
}
