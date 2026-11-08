using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopMenuController : Menu
{
    [SerializeField] TMP_Text thymeText;
    [SerializeField] TMP_Text fieldText;
    [Space]
    [SerializeField] ShopPurchaseButton itemButtonPrefab;
    [SerializeField] UpgradeList upgradeList;
    [SerializeField] Transform buttonParent;
    private readonly List<ShopPurchaseButton> activeButtons = new();

    protected override void Open()
    {
        base.Open();

        if (SaveSystem.TryGetSaveData(out var data))
        {
            thymeText.text = data.ThymeCollected.ToString();
            fieldText.text = data.NumFieldUpgrades.ToString();
        }

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

            saveData.ThymeCollected = (uint)Mathf.Max(0, saveData.ThymeCollected - button.GetPrice(saveData.GetUpgradeRepeatCount(button.Item)));

            saveData.TimerMultiplier *= button.Item.TimerMultiplier;
            saveData.RangeMultiplier *= button.Item.RangeMultiplier;
            saveData.SpeedMultiplier *= button.Item.SpeedMultiplier;

            saveData.TimerAdditive += button.Item.TimerAdditive;
            saveData.RangeAdditive += button.Item.RangeAdditive;
            saveData.SpeedAdditive += button.Item.SpeedAdditive;
            saveData.CatAdditive += button.Item.CatAdditive;
            saveData.ThymeAdditive += button.Item.ThymeAdditive;

            saveData.NumFieldUpgrades = (uint)Mathf.Max(0, saveData.NumFieldUpgrades + button.Item.NumFieldUpgrades);

            SaveSystem.UpdateSaveData(saveData);

            if (button.Item.ThymeAdditive > 0) PlantManager.AddFieldUpgrade();
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
