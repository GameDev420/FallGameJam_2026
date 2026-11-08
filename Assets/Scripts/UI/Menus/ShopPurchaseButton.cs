using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ShopPurchaseButton : MonoBehaviour
{
    public struct InitData
    {
        public UpgradeData Item;
        public OnClicked OnClick;
        public InitData(UpgradeData _Item, OnClicked _OnClick)
        {
            Item = _Item;
            OnClick = _OnClick;
        }
    }

    [SerializeField] TMP_Text itemNameText;
    //[SerializeField] TMP_Text itemDescriptionText;
    [SerializeField] TMP_Text itemPriceText;
    [SerializeField] TMP_Text itemThymeText;
    [SerializeField] Image itemIconImage;
    Button button;
    public Button Button => button;

    UpgradeData item;
    public UpgradeData Item => item;

    public delegate void OnClicked(ShopPurchaseButton button);
    UnityEngine.Events.UnityAction buttonActionCache;

    public void Init(InitData data)
    {
        if (button == null) button = GetComponent<Button>();
        
        item = data.Item;

        if (buttonActionCache != null) button.onClick.RemoveListener(buttonActionCache);
        
        buttonActionCache = () => data.OnClick.Invoke(this);
        button.onClick.AddListener(buttonActionCache);

        itemNameText.text = data.Item.DisplayName;
        //itemDescriptionText.text = data.Item.Description;
        itemPriceText.text = data.Item.Price.ToString();
        itemThymeText.text = data.Item.RequiredThyme.ToString();
        if (itemIconImage != null && data.Item.Icon != null) itemIconImage.sprite = data.Item.Icon;
    }

    // Should probably be moved to UpgradeData but for now this is fine
    public int GetPrice(int purchaseCount)
    {
        if (item == null) return 0;

        if (item.Repeatable && purchaseCount > 0)
            return Mathf.RoundToInt(item.Price * Mathf.Pow(item.RepeatPriceMultiplier, purchaseCount - 1));

        return item.Price;
    }

    public int GetThyme(int purchaseCount)
    {
        if (item == null) return 0;

        if (item.Repeatable && purchaseCount > 0)
            return Mathf.RoundToInt(item.RequiredThyme * Mathf.Pow(item.RepeatThymeMultiplier, purchaseCount - 1));

        return item.RequiredThyme;
    }
}
