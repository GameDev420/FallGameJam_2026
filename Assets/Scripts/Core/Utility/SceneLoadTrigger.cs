using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] int sceneIndex = 1;
    [SerializeField] string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) SceneLoader.Instance.LoadScene(sceneIndex);
    }
}
