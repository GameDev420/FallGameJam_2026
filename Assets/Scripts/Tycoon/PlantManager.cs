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
                var index = random ? Random.Range(0, emptyFields.Count) : 0;
                var field = emptyFields[index];
                field.sprite = filledFieldSprite;
                emptyFields.RemoveAt(index);
            }
        }
    }

    public static void AddFieldUpgrade()
    {
        if (instance == null) return;

        if (instance.emptyFields.Count <= 0) return;
        var index = instance.random ? Random.Range(0, instance.emptyFields.Count) : 0;
        var field = instance.emptyFields[index];
        field.sprite = instance.filledFieldSprite;
        instance.emptyFields.RemoveAt(index);
    }
}
