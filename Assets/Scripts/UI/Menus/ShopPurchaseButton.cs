using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ShopPurchaseButton : MonoBehaviour
{
    public struct InitData
    {
        public UpgradeData Item;
        public InitData(UpgradeData _Item)
        {
            Item = _Item;
        }
    }

    [SerializeField] TMP_Text itemNameText;
    //[SerializeField] TMP_Text itemDescriptionText;
    [SerializeField] TMP_Text itemPriceText;
    Button button;
    public Button Button => button;

    public void Init(InitData data)
    {
        button = GetComponent<Button>();

        itemNameText.text = data.Item.DisplayName;
        //itemDescriptionText.text = data.Item.Description;
        itemPriceText.text = data.Item.Price.ToString();
    }
}
