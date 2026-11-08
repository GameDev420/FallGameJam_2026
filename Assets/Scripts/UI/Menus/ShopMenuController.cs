using UnityEngine;
using System.Collections.Generic;

public class ShopMenuController : Menu
{
    [SerializeField] ShopPurchaseButton itemButtonPrefab;
    [SerializeField] UpgradeList upgradeList;
    [SerializeField] Transform buttonParent;
    private readonly List<ShopPurchaseButton> activeButtons = new();

    protected override void Open()
    {
        base.Open();

        var remainingUpgrades = GetRemainingUpgrades();
        if (remainingUpgrades == null) return;

        for (int i = 0; i < remainingUpgrades.Count; i++)
        {
            ShopPurchaseButton button;
            if (i < activeButtons.Count) button = activeButtons[i];
            else
            {
                button = Instantiate(itemButtonPrefab, buttonParent);
                activeButtons.Add(button);
            }

            button.Init(new(remainingUpgrades[i], OnPurchaseButtonClicked));
        }

        if (remainingUpgrades.Count < activeButtons.Count)
        {
            for (int i = remainingUpgrades.Count; i < activeButtons.Count; i++)
            {
                Destroy(activeButtons[i].gameObject);
            }
            activeButtons.RemoveRange(remainingUpgrades.Count, activeButtons.Count - remainingUpgrades.Count);
        }
    }

    void OnPurchaseButtonClicked(ShopPurchaseButton button)
    {
        if (SaveSystem.TryGetSaveData(out var saveData))
        {
            saveData.UnlockedUpgrades.Add(button.Item);
            SaveSystem.UpdateSaveData(saveData);
        }
        Destroy(button.gameObject);
    }

    // Ugly ahh mess but works for now
    List<UpgradeData> GetRemainingUpgrades()
    {
        if (upgradeList == null) return null;

        var remaining = new List<UpgradeData>(upgradeList.AllUpgrades);

        if (SaveSystem.TryGetSaveData(out var saveData) && saveData.UnlockedUpgrades != null && saveData.UnlockedUpgrades.Count > 0)
        {
            for (int i = 0; i < saveData.UnlockedUpgrades.Count; i++)
            {
                var index = remaining.IndexOf(saveData.UnlockedUpgrades[i]);
                if (index != -1 || saveData.UnlockedUpgrades[i].Repeatable)
                {
                    // Check if upgrade can be purchased based on prerequisites
                    var prerequisites = saveData.UnlockedUpgrades[i].Prerequisites;
                    if (prerequisites != null && prerequisites.Length > 0)
                    {
                        var canPurchase = true;
                        foreach (var prerequisite in prerequisites)
                        {
                            if (!saveData.UnlockedUpgrades.Contains(prerequisite))
                            {
                                canPurchase = false;
                                break;
                            }
                        }
                        if (canPurchase) remaining.RemoveAt(index);
                    }
                }
            }
        }

        return remaining;
    }
}
