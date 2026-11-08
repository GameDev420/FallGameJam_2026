using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class ThymeCollector : MonoBehaviour
{
    [SerializeField] TMP_Text thymeText;

    uint thymeCollected = 0; // this run

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Thyme"))
        {
            thymeCollected++;
            thymeText.text = thymeCollected.ToString();
            
            collision.gameObject.SetActive(false);
        }
    }
}
