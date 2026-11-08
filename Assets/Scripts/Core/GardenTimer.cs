using UnityEngine;
using TMPro;

public class GardenTimer : MonoBehaviour
{
    [SerializeField] int tycoonSceneIndex = 2;
    [SerializeField] private float timer = 60f;
    [SerializeField] private TMP_Text timerText;
    bool ended = false;

    private void Start()
    {
        timer *= SaveSystem.Instance.Data.TimerMultiplier;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && !ended)
        {
            timerText.text = "0";
            ended = true;

            SceneLoader.Instance.LoadScene(tycoonSceneIndex);
        }
        else timerText.text = Mathf.CeilToInt(timer).ToString();
    }
}
