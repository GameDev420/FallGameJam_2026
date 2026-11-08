using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShopTrigger : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] ShopMenuController shopMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) shopMenu.Open();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) shopMenu.Close();
    }
}
