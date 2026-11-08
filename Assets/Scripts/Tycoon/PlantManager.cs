using UnityEngine;
using System.Collections.Generic;

public class PlantManager : MonoBehaviour
{
    static PlantManager instance;

    [SerializeField] bool random;
    [SerializeField] Sprite filledFieldSprite;
    [SerializeField] List<SpriteRenderer> emptyFields = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start() => InitPlants();

    void InitPlants()
    {
        if (SaveSystem.TryGetSaveData(out var data))
        {
            for (int i = 0; i < data.NumFieldUpgrades; i++)
            {
                if (emptyFields.Count <= 0) break;
                var field = emptyFields[random ? Random.Range(0, emptyFields.Count) : 0];
                field.sprite = filledFieldSprite;
                emptyFields.RemoveAt(0);
            }
        }
    }

    public static void AddFieldUpgrade()
    {
        if (instance == null) return;
        if (instance.emptyFields.Count <= 0) return;
        var field = instance.emptyFields[instance.random ? Random.Range(0, instance.emptyFields.Count) : 0];
        field.sprite = instance.filledFieldSprite;
        instance.emptyFields.RemoveAt(0);
    }
}
