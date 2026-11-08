using UnityEngine;
using TMPro;

public class ThymeCollector1 : MonoBehaviour
{
    [SerializeField] float baseCollectRadius = 1.0f;
    [SerializeField] LayerMask collectMask;
    [SerializeField] TMP_Text thymeText;
    uint localThymeCollected = 0; // this run

    private void Awake()
    {
        thymeText.text = localThymeCollected.ToString();
    }

    private void Start()
    {
        baseCollectRadius *= Mathf.Max(1f, SaveSystem.Instance.Data.RangeMultiplier);
        baseCollectRadius += SaveSystem.Instance.Data.RangeAdditive;
    }

    private void FixedUpdate()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, baseCollectRadius, collectMask);
        foreach (var collider in colliders)
        {
            if (!collider.isActiveAndEnabled) continue;
            
            (collider.attachedRigidbody != null ? collider.attachedRigidbody.gameObject : collider.gameObject).SetActive(false);

            localThymeCollected++;
            thymeText.text = localThymeCollected.ToString();

            OnCollect();
        }
    }

    void OnCollect()
    {
        if (SaveSystem.TryGetSaveData(out var saveData))
        {
            saveData.ThymeCollected++;
            SaveSystem.UpdateSaveData(saveData);
        }
    }
}
